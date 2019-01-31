﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Bot.Builder.Dialogs.Tests
{
    public class TestUtilities
    {
        public static TurnContext CreateEmptyContext(TestContext testContext)
        {
            var b = new TestAdapter(TestAdapter.CreateConversation(testContext.TestName));
            var a = new Activity
            {
                Type = ActivityTypes.Message,
                ChannelId = "EmptyContext",
                From = new ChannelAccount
                {
                    Id = "empty@empty.context.org",
                }, 
                Conversation = new ConversationAccount()
                {
                    Id="213123123123"
                }
            };
            var bc = new TurnContext(b, a);

            return bc;
        }

        static Lazy<Dictionary<string, string>> environmentKeys = new Lazy<Dictionary<string, string>>(() =>
        {
            try
            {
                return File.ReadAllLines(@"\\fusebox\private\sdk\UnitTestKeys-new.cmd")
                    .Where(l => l.StartsWith("@set", StringComparison.OrdinalIgnoreCase))
                    .Select(l => l.Replace("@set ", string.Empty, StringComparison.OrdinalIgnoreCase).Split('='))
                    .ToDictionary(pairs => pairs[0], pairs => pairs[1]);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                return new Dictionary<string, string>();
            }
        });

        public static string GetKey(string key)
        {
            if (!environmentKeys.Value.TryGetValue(key, out var value))
            {
                // fallback to environment variables
                value = Environment.GetEnvironmentVariable(key);
                if (string.IsNullOrWhiteSpace(value))
                    value = null;
            }
            return value;
        }
    }
}
