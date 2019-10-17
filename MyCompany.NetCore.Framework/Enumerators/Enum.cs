namespace MyCompany.NetCore.Framework.Enumerators
{
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        PATCH,
        HEAD,
        OPTIONS
    }
    public enum AuthProtocol
    {
        BASIC,
        NTLM
    }
    public enum ApiType
    {
        SOAP,
        REST,
        JMS
    }
    public enum ResponseType
    {
        TEXT,
        XML,
        JSON,
        HTML
    }
    public enum AuthType
    {
        BASIC,
        JWT,
        BEARER,
        NO,
    }
}
