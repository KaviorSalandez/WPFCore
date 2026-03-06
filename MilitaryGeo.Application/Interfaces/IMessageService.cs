using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryGeo.Application.Interfaces
{
    public interface IMessageService
    {
        void ShowInfo(string message, string title = "Thông báo");
        void ShowError(string message, string title = "Lỗi hệ thống");
        bool ShowConfirmation(string message, string title = "Xác nhận");
    }
}
