export class Constants {
  public static EMAIL_REGEXP =
    /^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@[a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*/;
  public static PASSWORD_REGEXP =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$/;
  public static PHONE_REGEXP = /^\d{10}$/;
  public static SPECIAL_CHAR_NUMBER = /^\d+$/;
  public static SPECIAL_CHAR_CHARACTER = /^[a-zA-Z-'.\s ]$/;
  public static SPECIAL_CHAR_CHARACTERS = /^[a-zA-Z-'.\s ]{3,20}$/;
  public static SPECIAL_NOSPACE_NAME = /^\s*(?:\S\s*){3,20}$/;
  public static SPECIAL_ALPHANUMERIC = /^[a-zA-Z0-9\.\-]+./;
  public static ALPHANUMERIC = /^[a-zA-Z ][0-9]*/;
  public static OTP_NUMBERS_REGEXP = /^[0-9]{6}$/;
  public static gstPattern = /\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}/;
  public static mobileNoPattern = "^((\\+1-?)|0)?[0-9]{9,13}$";
  public static zipCode = "^[1-9][0-9]{5}$";
}


