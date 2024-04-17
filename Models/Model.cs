using System;
using System.Collections.Generic;

namespace SBM_POWER_BI.Models
{
    public class LoginModel
    {
        public string WINDOWS_ID { get; set; }
        public string PASSWORD { get; set; }
    }
    public class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }

    }
    public class UserInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string Manager { get; set; }
        public string EmployeeID { get; set; }
        public string Office { get; set; }
        // Add more properties for additional attributes
    }
    public class GET_DIALY_METHOD
    {
        public string DATE { get; set; }
        public string METODE { get; set; }
        public string TOTAL { get; set; }
    }
    public class GET_TOP_APPUSAGE
    {
        public string TEXT { get; set; }
        public string NUMBER { get; set; }
    }
    public class GET_TOP_VISITOR
    {
        public string VISITOR { get; set; }
        public string METHOD { get; set; }
        public string TOTAL_VISIT { get; set; }
    }
    public class CHART_MODEL
    {
        public List<CHART_1> CHART_1 { get; set; }
        public List<CHART_2> CHART_2 { get; set; }
        public List<CHART_3> CHART_3 { get; set; }
        public List<CHART_4> CHART_4 { get; set; }
        public List<CHART_5> CHART_5 { get; set; }
        public List<CHART_6> CHART_6 { get; set; }
    }
    public class CHART_1
    {
        public string REQUESTEDURI { get; set; }
        public string REQUESTCOUNT { get; set; }

    }
    public class CHART_2
    {
        public string HTTPMETHOD { get; set; }
        public string AVERAGERESPONSETIME { get; set; }
    }
    public class CHART_3
    {
        public string USERAGENT { get; set; }
        public string REQUESTCOUNT { get; set; }
    }
    public class CHART_4
    {
        public string CLIENTIPADDRESS { get; set; }
        public string HTTPSTATUS { get; set; }
        public string ERRORCOUNT { get; set; }
    }
    public class CHART_5
    {
        public string HOUROFDAY { get; set; }
        public string REQUESTCOUNT { get; set; }
    }
    public class CHART_6
    {
        public string REQUESTEDURI { get; set; }
        public string REFERRINGPAGE { get; set; }
        public string REFERRALCOUNT { get; set; }
    }
    //
    public class GET_HTTP_STATUS_CODE_DISTRIBUTION
    {
        public string DATE { get; set; }
        public string CODE { get; set; }
        public string DESC { get; set; }
        public string TOTAL { get; set; }
    }
    public class GET_REQUEST_METHODS_BREAKDOWN
    {
        public string REQUEST_METHOD { get; set; }
        public string DATE { get; set; }
        public int USAGE_COUNT { get; set; }
        public int TOTAL_REQUESTS { get; set; }
        public decimal PERCENTAGE_OF_USAGE { get; set; }
        public int PREVIOUS_USAGE { get; set; }
        public int DAY_NUMBER { get; set; }
        public int MAX_USAGE { get; set; }
        public int MIN_USAGE { get; set; }
        public int AVG_USAGE { get; set; }
    }
    public class GET_TOP_REQUESTED_URIS
    {
        public int RANK { get; set; }
        public string URL { get; set; }
        public int USAGECOUNT { get; set; }
        public int MAXSTATUSCODE { get; set; }
        public int MINSTATUSCODE { get; set; }
        public decimal AVGRESPONSETIME { get; set; }
        public long TOTALRESPONSESIZE { get; set; }
    }
    public class GET_USER_ACTIVITY_BY_DAY_OF_WEEK
    {
        public string DATE { get; set; }
        public int ACTIVITYCOUNT { get; set; }
        public double TOTALTIMETAKENHOURS { get; set; }
        public double AVGTIMETAKENHOURS { get; set; }
        public double MAXTIMETAKENHOURS { get; set; }
        public int ACTIVITYRANK { get; set; }
    }

    

    public class PBI_REPORT
    {
        public string ID { get; set; }
        public string REPORT_NAME { get; set; }
        public string REPORT_DESC { get; set; }
        public string REPORT_URL { get; set; }
        public string REPORT_URL_EXTERNAL { get; set; }
        public string REPORT_URL_PAGE { get; set; }
        public string REPORT_DEPT { get; set; }
        public string REPORT_CATEGORY { get; set; }
        public string REPORT_VISIBLE { get; set; }
        public string TOTAL_VIEW { get; set; }
        public string TOTAL_PAGE { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
        public string TAG_DEPT { get; set; }
        public List<PBI_REPORT_SUB> SubReports { get; set; }
    }

    public class PBI_REPORT_SUB
    {
        public string ID { get; set; }
        public string REPORT_PARENT { get; set; }
        public string REPORT_PAGE { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
    }

    public class PBI_CATEGORY
    {
        public string ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_DESC { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
    }

    public class PBI_DEPARTMENT
    {
        public string ID { get; set; }
        public string DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_DESC { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
    }

    public class PBI_GROUP
    {
        public string ID { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string GROUP_DESC { get; set; }
        public string ALLOW_CREATE { get; set; }
        public string ALLOW_SHARE { get; set; }
        public string ALLOW_VIEW { get; set; }
        public string ALLOW_EXPORT { get; set; }
        public string ACTIVE { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_DATE { get; set; }
    }

    public class PBI_USER
    {
        public string PLANT { get; set; }
        public string USERID { get; set; }
        public string WINDOWSID { get; set; }
        public string FULL_NAME { get; set; }
        public string DEPT { get; set; }
        public string EMAIL { get; set; }
        public string GROUPS_MDM { get; set; }
        public string GROUPS_PBI { get; set; }
    }

    public class PBI_USER_DC
    {
        public string samaccountname { get; set; }
        public string givenname { get; set; }
        public string surname { get; set; }
        public string mail { get; set; }
        public string telephonenumber { get; set; }
        public string department { get; set; }
        public string pbi_group { get; set; }
        public string company { get; set; }
        public string country { get; set; }
        public string userprincipalname { get; set; }
        public string lastlogontimestamp { get; set; }
        public string whencreated { get; set; }
        public string whenchanged { get; set; }
    }

    public class PBI_SHARE_URL
    {
        public string ID { get; set; }
        public string REPORT_NAME { get; set; }
        public string REPORT_ID { get; set; }
        public string URL_TYPE { get; set; }
        public string URL_TEXT { get; set; }
        public string URL_PASS { get; set; }
        public string URL_EXPI { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_DATE { get; set; }
    }
    public class MENU_LIST
    {
        public string SYSTEMNAME { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string PARENT { get; set; }
        public string CONTROLLER { get; set; }
        public string ACTION { get; set; }
        public string PARAMETER { get; set; }
        public string PICTURE { get; set; }
        public string ICON { get; set; }
        public string TARGET { get; set; }
        public string METHOD { get; set; }
        public string ADDITIONALINFO { get; set; }
        public string SEQUENCE { get; set; }
        public string STATUS { get; set; }
        public string CREATEUSER { get; set; }
        public string CREATEDATE { get; set; }
        public string CHANGEUSER { get; set; }
        public string CHANGEDATE { get; set; }
    }
}