namespace CrazySummerLab.Scripts
{
    public enum AntiaddictionType
    {
        USER_AGE_UNKNOWN,//user's age is unknown.
        USER_AGE_ZERO_IN_EiGHT,//user's age is in [0,8)
        USER_AGE_EIGHT_IN_SIXTEEN,//user's age is in [8,16)
        USER_AGE_SIXTEEN_IN_EIGHTEEN,//user's age is in [16,18)
        USER_AGE_ADULT,//user's age is in more than 18
    }
}
