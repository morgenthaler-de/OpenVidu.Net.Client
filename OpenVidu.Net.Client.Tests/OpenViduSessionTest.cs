using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenVidu.Net.Client.Tests
{
    [TestClass]
    public class OpenViduSessionTest
    {
        private const string BaseUrl = "";
        private const string Secret = "";

        [TestMethod]
        public async Task Create_And_Delete_Session_With_Token_Check()
        {
            var openVidu = new OpenVidu(BaseUrl, Secret);
            var session = openVidu.createSession();

            //session was successfully created
            Assert.IsTrue(session.hasSessionId());

            //receive token for session
            var token = await session.generateToken();

            //check if token was received
            Assert.IsNotNull(token);
            Assert.AreNotEqual("",token);

            var changed = await session.fetch();

            //remove session
            var closed = await session.close();
            Assert.IsTrue(closed);
        }

        [TestMethod]
        public async Task Create_And_Get_Active_Session()
        {
            var openVidu = new OpenVidu(BaseUrl, Secret);
            var session = openVidu.createSession();

            //session was successfully created
            Assert.IsTrue(session.hasSessionId());

            //Get active Sessions
            var sessions = openVidu.getActiveSessions();
            Assert.IsTrue(sessions.Count > 0);

            //check if created session is in the list of all sessions
            var matchedSession = sessions.Find(s => s.getSessionId() == session.getSessionId());
            Assert.IsNotNull(matchedSession);

            //remove session
            var closed = await session.close();
            Assert.IsTrue(closed);
        }
    }
}
