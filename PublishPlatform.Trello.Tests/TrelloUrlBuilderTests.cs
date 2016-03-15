using FluentAssertions;
using NUnit.Framework;

namespace PublishPlatform.Trello.Tests
{
    [TestFixture]
    public class TrelloUrlBuilderTests
    {


        [Test]
        public void Builder_WhenTokenSpecified_BuildsExpectedUrl()
        {
            var result = TrelloUrlBuilder.With()
                                        .Token("test")
                                        .Build();

            result.Should().Be("https://api.trello.com/1/no_action_specified?key=test");
        }

        [Test]
        public void Builder_WhenAuthTokenSpecified_BuildsExpectedUrl()
        {
            var result = TrelloUrlBuilder.With()
                                        .Token("testToken")
                                        .Auth("testAuth")
                                        .Build();

            result.Should().Be("https://api.trello.com/1/no_action_specified?key=testToken&token=testAuth");
        }

        [Test]
        public void Builder_WhenNoTokenSpecified_NoTokenOrAuthReturned()
        {
            var result = TrelloUrlBuilder.With()
                                        .Build();

            result.Should().Be("https://api.trello.com/1/no_action_specified");
        }

        [Test]
        public void Builder_QueryStringHasBeenSpecified_TokenSetCorrectly()
        {
            var result = TrelloUrlBuilder.With()
                                        .Action("/member/me?test=test")
                                        .Token("testToken")
                                        .Build();

            result.Should().Be("https://api.trello.com/1/member/me?test=test&key=testToken");
        }

        [Test]
        public void Builder_BuildsExpectedUrl()
        {
            var result = TrelloUrlBuilder.With()
                                        .Action("/member/me")
                                        .Token("testToken")
                                        .Auth("testAuth")
                                        .Build();

            result.Should().Be("https://api.trello.com/1/member/me?key=testToken&token=testAuth");
        }
    }
}
