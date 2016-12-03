using MongoDB.Bson;

namespace MyMoods.Controllers.Analytics
{
    public class AnalyticsBaseController : BaseController
    {
        public string LoggedUserId => Request.Headers["X-User"].ToString();

        public string LoggedCompanyId => Request.Headers["X-Company"].ToString();

        public short ClientTimezone => short.Parse(Request.Headers["X-Timezone"].ToString());

        public ObjectId LoggedUserOid => new ObjectId(LoggedUserId);

        public ObjectId LoggedCompanyOid => new ObjectId(LoggedCompanyId);
    }
}
