﻿using MongoDB.Bson;

namespace MyMoods.Controllers.Analytics
{
    public class AnalyticsBaseController : BaseController
    {
        public string LoggedCompanyId => Request.Headers["X-Company"].ToString();

        public ObjectId LoggedCompanyOid => new ObjectId(LoggedCompanyId);
    }
}
