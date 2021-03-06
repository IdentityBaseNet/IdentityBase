// Copyright (c) Russlan Akiev. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityBase
{
    using System;
    using System.Linq;
    using IdentityBase.Models;

    public static partial class UserAccountExtensions
    {
        public static bool HasPassword(this UserAccount userAccount)
        {
            if (userAccount == null)
            {
                throw new ArgumentException(nameof(userAccount));
            }

            return !String.IsNullOrWhiteSpace(userAccount.PasswordHash);
        }

        public static bool HasExternalAccounts(this UserAccount userAccount)
        {
            return userAccount.Accounts != null &&
                userAccount.Accounts.Count() > 0;
        }

        public static bool IsNew(this UserAccount userAccount)
        {
            if (userAccount == null)
            {
                throw new ArgumentException(nameof(userAccount));
            }

            return !userAccount.LastLoginAt.HasValue;
        }

        public static string GetTwoFactorAuthInfo(this UserAccount userAccount)
        {
            return "..."; 
        }
    }
}
