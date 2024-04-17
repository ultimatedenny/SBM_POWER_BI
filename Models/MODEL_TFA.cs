using System;
using System.Collections.Generic;

namespace SBM_POWER_BI.Models
{
    public class ADD_SYSTEM_TFA
    {
        public string ID { get; set; }
        public string APPLICATION { get; set; }
        public string SECRET { get; set; }
        public bool IS_REQUIRE { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
    }
    public class ADD_USER_TFA
    {
        public string ID { get; set; }
        public string USERID { get; set; }
        public string EMAIL { get; set; }
        public string APPLICATION { get; set; }
        public string SECRET_CODE { get; set; }
        public string SECRET_TEXT { get; set; }
        public string SECRET_URL { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
    }
    public class MODEL_TFA
    {
        public string CODE { get; set; }
    }
}