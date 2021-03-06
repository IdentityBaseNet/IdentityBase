// Copyright (c) Russlan Akiev. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityBase.Actions.Recover
{
    using IdentityBase.Models;

    public class SuccessViewModel
    {
        public string ReturnUrl { get; set; }

        public EmailProviderInfo Provider { get; set; }
    }
}