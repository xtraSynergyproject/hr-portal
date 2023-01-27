using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class EditorTypeViewModel : EditorType
    {
        public DataActionEnum DataAction { get; set; }
        public string EditorCategoryData { get; set; }
        public string ControlTypeData { get; set; }
        public string IconCss
        {
            get
            {
                switch (ControlType)
                {
                    case ControlTypeEnum.TextBox:
                        return "fa fal fa-money-check-edit fa-2x";
                    case ControlTypeEnum.TextArea:
                        return "fa fal fa-window fa-2x";
                    case ControlTypeEnum.CheckBox:
                        return "fa fal fa-check-square fa-2x";
                    case ControlTypeEnum.DateTime:
                        return "fa fal fa-clock fa-2x";
                    case ControlTypeEnum.Email:
                        return "fa fal fa-envelope fa-2x";
                    case ControlTypeEnum.Numeric:
                        return "fa fal fa-money-check-edit fa-2x";
                    case ControlTypeEnum.Decimal:
                        return "fa fal fa-money-check-edit fa-2x";
                    case ControlTypeEnum.Switch:
                        return "fa fal fa-sliders-v-square fa-2x";
                    case ControlTypeEnum.CheckBoxList:
                        return "fa fal fa-tasks fa-2x";
                    case ControlTypeEnum.RadioButtonList:
                        return "fa fal fa-dot-circle fa-2x";
                    case ControlTypeEnum.DropDownList:
                        return "fa fal fa-indent fa-2x";
                    case ControlTypeEnum.ListView:
                        return "fa fal fa-list-alt fa-2x";
                    case ControlTypeEnum.ColorPicker:
                        return "fa fal fa-eye-dropper fa-2x";
                    case ControlTypeEnum.GridLayout:
                        return "fa fal fa-th fa-2x";
                    case ControlTypeEnum.RichTextBox:
                        return "fa fal fa-window-maximize fa-2x";
                    default:
                        return "fa fal fa-edit fa-2x";
                }
            }
        }
    }
}
