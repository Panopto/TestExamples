using FakeDBLibrary;
using System;

namespace FakeProductLibrary
{
    public class DemoClass
    {
        const int MinimumPasswordLength = 8;
        const int PasswordHistoryMax = 5;
        static readonly char[] Numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static readonly char[] SpecialCharacters = new char[] { '`', '~', '!', '@', '#', '$', '%', '^', '&',
            '*', '(', ')', '_', '-','+', '=', '\\', '|', ';', ':', '\'', '"', ',', '<', '.', '>', '/', '?'};

        private IPanoptoDBDataContext DB;

        [Flags]
        public enum PasswordChangeStatus
        {
            Success = 0x00,
            LengthRequirement = 0x01,
            UpperCaseLetterRequirement = 0x02,
            NumberRequirement = 0x04,
            SpecialCharacterRequirement = 0x08,
            HistoryRequirement = 0x16
        }

        public DemoClass()
        {

        }

        public DemoClass(IPanoptoDBDataContext db)
        {
            DB = db;
        }

        /// <summary>
        /// Change password if it meets certain conditions
        /// </summary>
        /// <param name="newPassword">The new password</param>
        /// <returns>PasswordChangeStatus indicating success or failure based on conditions</returns>
        public PasswordChangeStatus ChangePassword(IPanoptoDBDataContext db, string newPassword)
        {
            PasswordChangeStatus status = PasswordChangeStatus.Success;
            
            if (newPassword.Length < MinimumPasswordLength)
            {
                status |= PasswordChangeStatus.LengthRequirement;
            }

            if (newPassword == newPassword.ToLower())
            {
                status |= PasswordChangeStatus.UpperCaseLetterRequirement;
            }

            if (newPassword.IndexOfAny(Numbers) < 0)
            {
                status |= PasswordChangeStatus.NumberRequirement;
            }

            if (newPassword.IndexOfAny(SpecialCharacters) < 0)
            {
                status |= PasswordChangeStatus.SpecialCharacterRequirement;
            }

            if (db.PasswordHistoryMatches(newPassword, PasswordHistoryMax))
            {
                status |= PasswordChangeStatus.HistoryRequirement;
            }

            // must meet all requirements or miss only one of uppercase letter, number, or special character
            if (status == PasswordChangeStatus.Success ||
                status == PasswordChangeStatus.UpperCaseLetterRequirement ||
                status == PasswordChangeStatus.NumberRequirement ||
                status == PasswordChangeStatus.SpecialCharacterRequirement)
            {
                db.SetPassword(newPassword);
                db.SubmitChanges();
                status = PasswordChangeStatus.Success;
            }

            return status;
        }
        
        public int Power(int baseNumber, int exponent)
        {
            int result = 1;
            while (exponent > 0)
            {
                if ((exponent & 1) != 0)
                    result *= baseNumber;
                exponent >>= 1;
                baseNumber *= baseNumber;
            }
            return result;
        }

        /// <summary>
        /// Change password if it meets certain conditions, example with test PanoptoDBDataContext from constructor.
        /// </summary>
        /// <param name="newPassword">The new password</param>
        /// <returns>PasswordChangeStatus indicating success or failure based on conditions</returns>
        public PasswordChangeStatus ChangePassword(string newPassword)
        {
            PasswordChangeStatus status = PasswordChangeStatus.Success;

            if (newPassword.Length < MinimumPasswordLength)
            {
                status |= PasswordChangeStatus.LengthRequirement;
            }

            if (newPassword == newPassword.ToLower())
            {
                status |= PasswordChangeStatus.UpperCaseLetterRequirement;
            }

            if (newPassword.IndexOfAny(Numbers) < 0)
            {
                status |= PasswordChangeStatus.NumberRequirement;
            }

            if (newPassword.IndexOfAny(SpecialCharacters) < 0)
            {
                status |= PasswordChangeStatus.SpecialCharacterRequirement;
            }

            // Really at this point you don't need the using statement, let the caller or a container control disposing
            using (IPanoptoDBDataContext db = DB ?? new PanoptoDBDataContext())
            {
                if (db.PasswordHistoryMatches(newPassword, PasswordHistoryMax))
                {
                    status |= PasswordChangeStatus.HistoryRequirement;
                }

                // must meet all requirements or miss only one of uppercase letter, number, or special character
                if (status == PasswordChangeStatus.Success ||
                    status == PasswordChangeStatus.UpperCaseLetterRequirement ||
                    status == PasswordChangeStatus.NumberRequirement ||
                    status == PasswordChangeStatus.SpecialCharacterRequirement)
                {
                    db.SetPassword(newPassword);
                    status = PasswordChangeStatus.Success;
                }
            }

            return status;
        }
    }
}
