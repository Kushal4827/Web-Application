using System;

namespace VDEM.Lib.Database
{
    public class TKM_APP_DETAILS
    {
        public TKM_APP_DETAILS()
        {

        }

        /// Column Comment...
        private decimal _ID;
        public virtual decimal ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }

        /// Column Comment...
        private string _APPLICATION_NAME;
        public virtual string APPLICATION_NAME
        {
            get
            {
                return this._APPLICATION_NAME;
            }
            set
            {
                this._APPLICATION_NAME = value;
            }
        }


        /// Column Comment...
        private string _APPLIATION_CATEGORY;
        public virtual string APPLIATION_CATEGORY
        {
            get
            {
                return this._APPLIATION_CATEGORY;
            }
            set
            {
                this._APPLIATION_CATEGORY = value;
            }
        }

        /// Column Comment...
        private string _PRODUCTION_URL;
        public virtual string PRODUCTION_URL
        {
            get
            {
                return this._PRODUCTION_URL;
            }
            set
            {
                this._PRODUCTION_URL = value;
            }
        }

        /// Column Comment...
        private string _TEST_URL;
        public virtual string TEST_URL
        {
            get
            {
                return this._TEST_URL;
            }
            set
            {
                this._TEST_URL = value;
            }
        }

        /// Column Comment...
        private string _PURPOSE;
        public virtual string PURPOSE
        {
            get
            {
                return this._PURPOSE;
            }
            set
            {
                this._PURPOSE = value;
            }
        }

        /// Column Comment...
        private string _APPLICATION_TYPE;
        public virtual string APPLICATION_TYPE
        {
            get
            {
                return this._APPLICATION_TYPE;
            }
            set
            {
                this._APPLICATION_TYPE = value;
            }
        }

        /// Column Comment...
        private string _PRIMARY_SUPPORT_EMAIL;
        public virtual string PRIMARY_SUPPORT_EMAIL
        {
            get
            {
                return this._PRIMARY_SUPPORT_EMAIL;
            }
            set
            {
                this._PRIMARY_SUPPORT_EMAIL = value;
            }
        }

        /// Column Comment...
        private string _PRIMARY_SUPPORT_PHONE;
        public virtual string PRIMARY_SUPPORT_PHONE
        {
            get
            {
                return this._PRIMARY_SUPPORT_PHONE;
            }
            set
            {
                this._PRIMARY_SUPPORT_PHONE = value;
            }
        }

        /// Column Comment...
        private string _PRIMARY_SUPPORT_TEAMS;
        public virtual string PRIMARY_SUPPORT_TEAMS
        {
            get
            {
                return this._PRIMARY_SUPPORT_TEAMS;
            }
            set
            {
                this._PRIMARY_SUPPORT_TEAMS = value;
            }
        }

        /// Column Comment...
        private string _SECONDARY_SUPPORT_EMAIL;
        public virtual string SECONDARY_SUPPORT_EMAIL
        {
            get
            {
                return this._SECONDARY_SUPPORT_EMAIL;
            }
            set
            {
                this._SECONDARY_SUPPORT_EMAIL = value;
            }
        }

        /// Column Comment...
        private string _SECONDARY_SUPPORT_PHONE;
        public virtual string SECONDARY_SUPPORT_PHONE
        {
            get
            {
                return this._SECONDARY_SUPPORT_PHONE;
            }
            set
            {
                this._SECONDARY_SUPPORT_PHONE = value;
            }
        }

        /// Column Comment...
        private string _SECONDARY_SUPPORT_TEAMS;
        public virtual string SECONDARY_SUPPORT_TEAMS
        {
            get
            {
                return this._SECONDARY_SUPPORT_TEAMS;
            }
            set
            {
                this._SECONDARY_SUPPORT_TEAMS = value;
            }
        }
    }
}
