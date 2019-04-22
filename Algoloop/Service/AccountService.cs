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

using Algoloop.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Algoloop.Service
{
    [DataContract]
    public class AccountService
    {
        private static readonly AccountModel[] _standardAccounts = new[]
        {
            new AccountModel() { Name = AccountModel.AccountType.Backtest.ToString() },
            new AccountModel() { Name = AccountModel.AccountType.Paper.ToString() }
        };

        [Browsable(false)]
        [DataMember]
        public List<AccountModel> Accounts { get; } = new List<AccountModel>();

        internal void Copy(AccountService accountsModel)
        {
            Accounts.Clear();
            Accounts.AddRange(accountsModel.Accounts);
        }

        internal AccountModel FindAccount(string account)
        {
            return Accounts.Find(m => m.Name.Equals(account));
        }

        internal IReadOnlyList<AccountModel> GetAccounts()
        {
            IEnumerable<AccountModel> accounts = Accounts.Concat(_standardAccounts);
            return accounts.ToList();
        }
    }
}
