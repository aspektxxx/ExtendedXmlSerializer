﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using ExtendedXmlSerializer.Tests.Support;
using FluentAssertions;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue230Tests
	{
		[Fact]
		void Verify()
		{
			var subject = new Subject { Message1 = "This is message." };

			var serializer = new ConfigurationContainer().AllowExistingInstances()
			                                             .Create()
			                                             .ForTesting();

			const string data = /* language=XML */ @"<?xml version=""1.0"" encoding=""utf-8""?><Issue230Tests-Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests""><Message2>This is another message.</Message2></Issue230Tests-Subject>";

			subject.Message2.Should().BeNull();

			var returned = serializer.Deserialize(subject, data);

			returned.Should().Be(subject);

			subject.Message2.Should().Be("This is another message.");
		}

		sealed class Subject
		{
			public string Message1 { get; set; }

			public string Message2 { get; set; }
		}
	}
}