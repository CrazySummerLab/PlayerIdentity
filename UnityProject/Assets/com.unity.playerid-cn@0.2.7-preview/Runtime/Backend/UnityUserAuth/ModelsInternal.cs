using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Scripting;

[assembly: Preserve]
namespace UnityEngine.PlayerIdentity.UnityUserAuth
{
    internal interface IApiResponseBase
    {
        Error GetError(object requestType);
   
    }


    internal class ResponseBase : ErrorResponse, IApiResponseBase
    {
        public static string SPECIAL_ERROR_TYPE = "INVALID_PARAMETERS";

        public static Dictionary<string, string> errorCorrespond = new Dictionary<string, string>()
        {
            {"INVALID_PARAMETERS", "无效的参数"}, {"ACCOUNT_EXISTS", "账户已存在"},
            {"INVALID_CREDENTIALS", "邮箱/密码 不正确"}, {"ACCOUNT_DISABLED", "该账户已被禁用"},
            {"INVALID_SESSION_TOKEN", "Session token 无效"}, {"ID_PROVIDER_ERROR", "无效的外部帐户credential或provider 类型"},
            {"TOKEN_EXPIRED", "Token 已过期"}, {"DATABASE_CONFLICT", "数据冲突"}, {"INVALID_REQUEST_CONTEXT", "请求无效"},
            {"INTERNAL_SERVER_ERROR", "server内部错误"}, {"UNAUTHORIZED_REQUEST", "未经授权的请求"}, {"PERMISSION_DENIED", "无权限"},
            {"RESOURCE_NOT_FOUND", "无该项资源"}, {"PASSWORD_AUTH_DISABLED", "密码认证被禁用"},
            {"PASSWORD_AUTH_ALREADY_SETUP", "密码认证已存在"},
            {"PASSWORD_AUTH_NOT_SETUP", "密码认证未建立"}, {"RESOURCE_EXHAUSTED", "该项资源暂无"}, {"INVALID_EMAIL", "该邮箱无效"},
            {"WEAK_PASSWORD", "密码安全程度较弱"}, {"INVALID_PASSWORD", "密码无效"}, {"USER_NOT_FOUND", "找不到该用户"},
            {"INVALID_GRANT_TYPE", "无效的授权类型"},
            {"MISSING_SESSION_TOKEN", "缺失Session Token"}, {"API_NOT_IMPLEMENTED", "该API 还未实现"},
            {"INVALID_ID_TYPE", "ID 类型无效"}, {"RESTRICTED_ACCESS", "已被禁用"}
        };

        public Error GetError()
        {
            return new Error
            {
                errorClass = ErrorClass.UserError,
                type = error,
                errorCodeType = GeterrorCodeType(message),
                message = GetErrorToChinese(message)
            };
        }

        public Error GetError(object requestType)
        {
            return new Error
            {
                errorClass = ErrorClass.UserError,
                type = error,
                errorCodeType = GeterrorCodeType(message),
                message = GetErrorToChinese(message, requestType)

            };
        }

        public static string Invalid_parameterError(object request)
        {
            if (request is ExternalTokenAuthRequest)
            {
                return "未指定的ID provider";
            }
            else if (request is UnlinkExternalIdRequest)
            {
                return "没有指定的ID provider 需要被取消链接";
            }
            else if (request is LinkSmsCodeRequest)
            {
                return "验证码为空";
            }
            else if (request is PasswordAuthRequest)
            {
                return "邮箱/密码 无效";
            }
            else if (request is SmsCodeAuthRequest)
            {
                return "验证码错误";
            }
            else
            {
                return "参数无效";
            }
        }

        public static string GetErrorToChinese(string message)
        {
            string errorType = GeterrorCodeType(message);
            return errorCorrespond.ContainsKey(errorType) ? errorCorrespond[errorType] : message;
        }

        public static string GetErrorToChinese(string message, object requestType)
        {
            string errorType = GeterrorCodeType(message);

            if (errorType == SPECIAL_ERROR_TYPE)
            {
                return Invalid_parameterError(requestType);
            }
            else
            {
                return errorCorrespond.ContainsKey(errorType) ? errorCorrespond[errorType] : message;
            }
        }

        public static string GeterrorCodeType(string message)
        {
            string type = message.Substring(0, message.IndexOf(":") == -1 ? 0 : message.IndexOf(":"));
            if (type == "")
            {
                return errorCorrespond.ContainsKey(message) ? message : "";
            }
            else
            {
                return type;
            }

        }
    }



    internal class ErrorResponse
    {
        public string error;
        public string message;
        public object[] details;
    }

    internal class PasswordAuthRequest
    {
        public string email;
        public string password;
    }

    internal class ExternalTokenAuthRequest
    {
        public string idProvider;
        public string idToken;
        public string accessToken;
        public string redirectUri;
        public string authCode;
        public string clientId;
        public string openid;
    }

    internal class SessionTokenAuthRequest
    {
        public string sessionToken;
    }

    internal class SignupRequest
    {
        public string email;
        public string password;
        public string displayName;
    }

    internal class CreateCredentialsRequest
    {
        public string email;
        public string password;
    }

    internal class ChangePasswordRequest
    {
        public string password;
        public string newPassword;
    }

    internal class UpdateUserRequest
    {
        public string id;
        public string displayName;
        public string photoUrl;
    }

    internal class AnonymousUserRequest
    {
    }
    
    internal class CreateSmsCodeResponse : ResponseBase
    {
        public string verificationId;
    }
    
        
    internal class SmsCodeAuthRequest
    {
        public string code;
        public string verificationId;
    }

    internal class CreateCodeRequest
    {
        public string phoneNumber;
    }

    internal class LinkExternalIdRequest
    {
        public string idProvider;
        public string idToken;
        public string accessToken;
        public string redirectUri;
    }

    internal class UnlinkExternalIdRequest
    {
        public string[] idProviders;
    }

    internal class LinkSmsCodeRequest
    {
        public string userId;
        public string code;
        public string verificationId;
    }
    
    internal class CreateCredentialRequest
    {
        public string email;
        public string password;
    }

    internal class ResetPasswordRequest
    {
        public string email;
    }

    internal class ResetPasswordResponse : ResponseBase
    {
    }
    
    internal class GetUserRequest
    {
        public string id;
    }
    
    internal class VerifyEmailRequest
    {
        public string email;
    }
    
    internal class VerifyEmailResponse : ResponseBase
    {}

    internal class ExternalIdResponse
    {
        public string providerId;
        public string externalId;
        public string displayName;
        public string email;
        public string phoneNumber;
    }

    internal class AuthenticationResponse : ResponseBase
    {
        public string userId;
        public string email;
        public string idToken;
        public string sessionToken;
        public int expiresIn;
        public bool needConfirmation;
        public User user;
        public string rawUserInfo;
    }

    internal class UserResponse : ResponseBase
    {
        public string id;
        public string idDomain;
        public bool disabled;
        public string displayName;
        public string email;
        public bool emailVerified;
        public Dictionary<string, string> metadata;
        public string photoUrl;
        public Dictionary<string, string> customClaims;
        public ExternalId[] externalIds;
    }
    
    internal class User
    {
        public string id;
        public string idDomain;
        public bool disabled;
        public string displayName;
        public string email;
        public bool emailVerified;
        public Dictionary<string, string> metadata;
        public string photoUrl;
        public Dictionary<string, string> customClaims;
        public ExternalId[] externalIds;
    }

    
    
    internal class ExternalId
    {
        public string providerId;
        public string externalId;
        public string displayName;
        public string email;
        public string phoneNumber;
    }

    
    internal class TokenRequest
    {
        public string grant_type;
        public string code;
        public string redirect_uri;
        public string scope;
        public string client_id;
        public string client_secret;
        public string refresh_token;
        public string code_verifier;
    }
    
    internal class JWTStandardClaims
    {
        public string iss;
        public string sub;
        public string[] aud;
        public long exp;
        public long nbf;
        public long iat;
        public string jti;
    }
    
    internal class IDToken : JWTStandardClaims
    {
        public string user_id;
        public string email;
        public bool email_verified;
        public string name;
        public string picture;
        public string sign_in_provider;
    }
    
    internal class AuthorizeRequest
    {
        public string response_type;
        public string client_id;
        public string redirect_uri;
        public string scope;
        public string state;
        public string nonce;
        public string prompt;
        public string id_token;
        public string code_challenge;
        public string code_challenge_method;
    }

    
    internal class OAuthResponseBase : IApiResponseBase
    {
        public string error;
        public string error_description;

        public Error GetError(object request)
        {
            return new Error
            {
                errorClass = ErrorClass.UserError,
                type = error,
                message = error_description,
            };
        }
    }

    internal class AuthorizeResponse : OAuthResponseBase
    {
        public string state;
        public string code;
        public string scope;
    }

    internal class TokenResponse : OAuthResponseBase
    {
        public string access_token;
        public string id_token;
        public string refresh_token;
        public string scope;
        public string token_type;
        public int expires_in;
    }
}
