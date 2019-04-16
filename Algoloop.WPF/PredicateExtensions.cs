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

using System;

namespace Algoloop.WPF
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> original, Predicate<T> newPredicate)
        {
            return t => original(t) && newPredicate(t);
        }

        public static Predicate<T> Or<T>(this Predicate<T> original, Predicate<T> newPredicate)
        {
            return t => original(t) || newPredicate(t);
        }
    }
}
