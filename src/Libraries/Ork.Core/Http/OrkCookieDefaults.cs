namespace Ork.Core.Http;

public static class OrkCookieDefaults
{
    /// <summary>
    /// Gets the cookie name prefix
    /// </summary>
    public const string Prefix = ".Ork";

    /// <summary>
    /// Gets a cookie name of the customer
    /// </summary>
    public const string CustomerCookie = ".Customer";

    /// <summary>
    /// Gets a cookie name of the antiforgery
    /// </summary>
    public const string AntiforgeryCookie = ".Antiforgery";

    /// <summary>
    /// Gets a cookie name of the session state
    /// </summary>
    public const string SessionCookie = ".Session";

    /// <summary>
    /// Gets a cookie name of the culture
    /// </summary>
    public const string CultureCookie = ".Culture";

    /// <summary>
    /// Gets a cookie name of the temp data
    /// </summary>
    public const string TempDataCookie = ".TempData";

    /// <summary>
    /// Gets a cookie name of the installation language
    /// </summary>
    public const string InstallationLanguageCookie = ".InstallationLanguage";

    /// <summary>
    /// Gets a cookie name of the compared products
    /// </summary>
    public const string ComparedProductsCookie = ".ComparedProducts";

    /// <summary>
    /// Gets a cookie name of the recently viewed products
    /// </summary>
    public const string RecentlyViewedProductsCookie = ".RecentlyViewedProducts";

    /// <summary>
    /// Gets a cookie name of the authentication
    /// </summary>
    public const string AuthenticationCookie = ".Authentication";

    /// <summary>
    /// Gets a cookie name of the external authentication
    /// </summary>
    public const string ExternalAuthenticationCookie = ".ExternalAuthentication";

    /// <summary>
    /// Gets a cookie name of the Eu Cookie Law Warning
    /// </summary>
    public const string IgnoreEuCookieLawWarning = ".IgnoreEuCookieLawWarning";
}
