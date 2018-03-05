using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Trawick.Rest.Helpers
{
	public class Valid
	{

		#region Email


		public static bool Email(string email)
		{
			string dummy;
			return Email(email, out dummy);
		}


		public static bool Email(string email, out string ErrorMessage)
		{
			ErrorMessage = string.Empty;

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("Email should not be empty");
			}

			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return true;
			}
			catch
			{
				ErrorMessage = "Email is invalid";
				return false;
			}
		}


		#endregion


		#region Password


		public static bool Password(string password)
		{
			string dummy;
			return Password(password, out dummy);
		}


		public static bool Password(string password, out string ErrorMessage)
		{
			ErrorMessage = string.Empty;

			const int MIN_LENGTH = 6;
			const int MAX_LENGTH = 11;

			const int MIN_LOWERS = 1;
			const int MIN_UPPERS = 1;
			const int MIN_DIGITS = 1;
			const int MIN_OTHERS = 0;

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentNullException("Password should not be empty");
			}
			if (password.Length < MIN_LENGTH)
			{
				ErrorMessage = string.Format("Password must be at least {0} characters long", MIN_LENGTH);
				return false;
			}
			if (MAX_LENGTH >= MIN_LENGTH && password.Length > MAX_LENGTH)
			{
				ErrorMessage = string.Format("Password must not be more than {0} characters long", MAX_LENGTH);
				return false;
			}

			var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
			//var hasSymbols = new Regex(@"[^\da-zA-Z]");// Any single char other than alpha or numeric

			int lowerCount = 0, upperCount = 0, digitCount = 0, otherCount = 0;

			foreach (char c in password)
			{
				if (char.IsUpper(c)) upperCount++;// hasUpperCaseLetter = true;
				if (char.IsLower(c)) lowerCount++;// hasLowerCaseLetter = true;
				if (char.IsDigit(c)) digitCount++;// hasDecimalDigit = true;
				if (hasSymbols.IsMatch(c.ToString())) otherCount++;
			}

			if (MIN_LOWERS > 0 && lowerCount < MIN_LOWERS)
			{
				//ErrorMessage = "Password must contain at least one lowercase letter";
				ErrorMessage = PasswordError(MIN_LOWERS, "lowercase letter");
				return false;
			}
			if (MIN_UPPERS > 0 && upperCount < MIN_UPPERS)
			{
				//ErrorMessage = "Password must contain at least one uppercase letter";
				ErrorMessage = PasswordError(MIN_UPPERS, "uppercase letter");
				return false;
			}
			if (MIN_DIGITS > 0 && digitCount < MIN_DIGITS)
			{
				//ErrorMessage = "Password must contain at least one numeric value";
				ErrorMessage = PasswordError(MIN_DIGITS, "numeric value");
				return false;
			}
			if (MIN_OTHERS > 0 && otherCount < MIN_OTHERS)
			{
				//ErrorMessage = "Password should contain At least one special character(s)";
				ErrorMessage = PasswordError(MIN_OTHERS, "special character");
				return false;
			}

			return true;
		}


		private static string PasswordError(int count = 0, string single = "", string plural = "")
		{
			string msg = string.Format("Password must contain at least {0} ", count);
			if (string.IsNullOrEmpty(plural))
				plural = single + "s";
			return msg + (count > 1 ? plural : single);
		}


		#endregion

	}
}


// https://stackoverflow.com/questions/5859632/regular-expression-for-password-validation

// The following regular expression can be used to match a string whose length is at least 6, 
// contains at least one digit and contains at least one lower case or upper case alphabet
// (?=^.{6,}$)(?=.*\d)(?=.*[a-zA-Z])

// However, I think it is good idea to avoid spaces in the string, in which case the following 
// regular expression can be used
// (?=^[^\s]{6,}$)(?=.*\d)(?=.*[a-zA-Z])

// This requires at least one digit, at least one alphabetic character, no special characters, 
// and from 6-15 characters in length.
// (?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$

// This requires at least one digit, at least one uppercase alphabetic character, at least one 
// lowercase alphabetic character, at least one special character, and from 8-15 characters in length.
// ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$