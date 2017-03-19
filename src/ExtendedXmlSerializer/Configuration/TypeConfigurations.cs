// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Reflection;
using ExtendedXmlSerialization.Core.Sources;

namespace ExtendedXmlSerialization.Configuration
{
	sealed class TypeConfigurations : ReferenceCacheBase<TypeInfo, IExtendedXmlTypeConfiguration>
	{
		public static IParameterizedSource<IExtendedXmlConfiguration, TypeConfigurations> Defaults { get; }
			= new ReferenceCache<IExtendedXmlConfiguration, TypeConfigurations>(x => new TypeConfigurations(x));

		readonly IExtendedXmlConfiguration _configuration;
		readonly IDictionary<TypeInfo, string> _names;

		TypeConfigurations(IExtendedXmlConfiguration configuration)
			: this(configuration, configuration.With<TypeNamesExtension>().Names) {}

		TypeConfigurations(IExtendedXmlConfiguration configuration, IDictionary<TypeInfo, string> names)
		{
			_configuration = configuration;
			_names = names;
		}

		protected override IExtendedXmlTypeConfiguration Create(TypeInfo parameter)
			=> new ExtendedXmlTypeConfiguration(_configuration, new TypeProperty<string>(_names, parameter), parameter);
	}

	sealed class TypeConfigurations<T> : ReferenceCache<IExtendedXmlConfiguration, ExtendedXmlTypeConfiguration<T>>
	{
		public static TypeConfigurations<T> Default { get; } = new TypeConfigurations<T>();
		TypeConfigurations() : base(x => new ExtendedXmlTypeConfiguration<T>(x)) {}

		public ExtendedXmlTypeConfiguration<T> For(IExtendedXmlTypeConfiguration type)
			=> type as ExtendedXmlTypeConfiguration<T> ?? Get(type.Configuration);
	}
}