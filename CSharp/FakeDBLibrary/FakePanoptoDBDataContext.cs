using System;

namespace FakeDBLibrary
{
    public interface IPanoptoDBDataContext: IDisposable
    {
        bool PasswordHistoryMatches(string passwordToCheck, int passwordHistoryMax);

        void SetPassword(string newPassword);

        void SubmitChanges();
    }

    public sealed class PanoptoDBDataContext : IPanoptoDBDataContext
    {
        // Some product library in another project that provides access to data layer, as a consumer of this class we
        //  don't necisarily need to unit test it, creator of this class unit tests it, we may still have some
        //  integration level tests that use the actual class

        #region IDisposable Support
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        public bool PasswordHistoryMatches(string passwordToCheck, int passwordHistoryMax)
        {
            // Some product code here, we're not testing this though so we don't care
            throw new NotImplementedException();
        }

        public void SetPassword(string newPassword)
        {
            // Some product code here, we're not testing this though so we don't care
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            // Some product code here, we're not testing this though so we don't care
            throw new NotImplementedException();
        }
    }
}
