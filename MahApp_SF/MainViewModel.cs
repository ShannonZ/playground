namespace MahApp_SF
{
    using System;
    using System.ComponentModel;
    using SoftFluent.Windows;

    class MainViewModel: AutoObject,IDataErrorInfo
    {
        [Category("Cannot goto Validation")]
        [DisplayName("TE")]
        [PropertyGridOptions(SortOrder = 1, EditorDataTemplateResourceKey = "TEEditor")]
        public float TE
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }



        public MainViewModel()
        {
            TE = 2;    // 
        }

        #region Validation
        [Browsable(false)]
        public string Error { get { return string.Empty; } }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "TE")
                {
                    //D4 
                    if (TE<20)
                    {
                        return "TE is too short ";
                    }
                   
                }

                return null;
            }
        }
        #endregion
    }
}
