﻿/*
 * Copyright 2019 Capnode AB
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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Algoloop.ViewModel
{
    public class FolderViewModel : ViewModelBase
    {
        private MarketViewModel _parent;

        public FolderViewModel(MarketViewModel market, FolderModel model)
        {
            _parent = market;
            Model = model;

            DeleteCommand = new RelayCommand(() => _parent?.DeleteFolder(this), () => !_parent.Active);
            RenameCommand = new RelayCommand(() => _parent?.DeleteFolder(this), () => !_parent.Active);
        }

        public FolderModel Model { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand RenameCommand { get; }
    }
}
