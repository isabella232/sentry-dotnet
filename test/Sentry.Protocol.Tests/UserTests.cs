using System.Collections.Generic;
using Xunit;

namespace Sentry.Protocol.Tests
{
    public class UserTests
    {
        [Fact]
        public void SerializeObject_AllPropertiesSetToNonDefault_SerializesValidObject()
        {
            var sut = new User
            {
                Id = "user-id",
                Email = "test@sentry.io",
                IpAddress = "::1",
                Username = "user-name",
                Extras = new Dictionary<string, string>
                            {
                                {"testCustomValueKey", "testCustomValue"}
                            }
            };

            var actual = JsonSerializer.SerializeObject(sut);

            Assert.Equal("{\"email\":\"test@sentry.io\","
                        + "\"id\":\"user-id\","
                        + "\"ip_address\":\"::1\","
                        + "\"username\":\"user-name\","
                        + "\"extras\":{\"testCustomValueKey\":\"testCustomValue\"}}",
                    actual);
        }

        [Fact]
        public void Clone_CopyValues()
        {
            var sut = new User()
            {
                Id = "id",
                Email = "emal@sentry.io",
                IpAddress = "::1",
                Username = "user",
                Extras = new Dictionary<string, string>
                            {
                                {"testCustomValueKey", "testCustomValue"}
                            }
            };

            var clone = sut.Clone();

            Assert.Equal(sut.Id, clone.Id);
            Assert.Equal(sut.Email, clone.Email);
            Assert.Same(sut.IpAddress, clone.IpAddress);
            Assert.Equal(sut.Username, clone.Username);
            Assert.Equal(sut.Extras, clone.Extras);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void SerializeObject_TestCase_SerializesAsExpected((User user, string serialized) @case)
        {
            var actual = JsonSerializer.SerializeObject(@case.user);

            Assert.Equal(@case.serialized, actual);
        }

        public static IEnumerable<object[]> TestCases()
        {
            yield return new object[] { (new User(), "{}") };
            yield return new object[] { (new User { Id = "some id" }, "{\"id\":\"some id\"}") };
            yield return new object[] { (new User { Email = "some email" }, "{\"email\":\"some email\"}") };
            yield return new object[] { (new User { IpAddress = "some ipAddress" }, "{\"ip_address\":\"some ipAddress\"}") };
            yield return new object[] { (new User { Username = "some username" }, "{\"username\":\"some username\"}") };
            yield return new object[] { (new User {Extras = new Dictionary<string, string>
                            {
                                {"testCustomValueKey", "testCustomValue"}
                            }

            }, "{\"extras\":{\"testCustomValueKey\":\"testCustomValue\"}}") };
        }
    }
}
