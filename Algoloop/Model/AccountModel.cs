﻿/*
 * Copyright 2018 Capnode AB
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using QuantConnect.Orders;

namespace Algoloop.Model
{
    [Serializable]
    [DataContract]
    public class AccountModel
    {
        public enum AccountType { Paper, Fxcm };
        public enum AccessType { No_login, Demo, Real };

        [DisplayName("Account name")]
        [Description("Name of the account.")]
        [DataMember]
        public string Name { get; set; } = "Account";

        [Browsable(false)]
        [DataMember]
        public bool Active { get; set; }

        [Category("Account")]
        [DisplayName("Account provider")]
        [Description("Name of the broker.")]
        [DataMember]
        public AccountType Account { get; set; }

        [Category("Account")]
        [DisplayName("Access type")]
        [Description("Type of login account at the broker.")]
        [DataMember]
        public AccessType Access { get; set; }

        [Category("Account")]
        [DisplayName("Login")]
        [Description("User login.")]
        [DataMember]
        public string Login { get; set; } = string.Empty;

        [Category("Account")]
        [DisplayName("Password")]
        [Description("User login password.")]
        [PasswordPropertyText(true)]
        [DataMember]
        public string Password { get; set; } = string.Empty;

        [Category("Account")]
        [DisplayName("Account number")]
        [Description("Account number.")]
        [DataMember]
        public string Id { get; set; } = string.Empty;

        [Browsable(false)]
        [DataMember]
        public string DataFolder { get; set; }

        [Browsable(false)]
        [DataMember]
        public List<OrderModel> Orders { get; } = new List<OrderModel>();
    }
}