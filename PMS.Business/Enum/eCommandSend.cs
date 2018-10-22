using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS.Business.Enum
{
    public enum eCommandSend
    {
        Poll = 4,
        ProductConfig = 9,
        ChangeProductQuantity = 10,
        ChangeProductError = 11,
        EndOfDay = 12,
        ChangeBTPQuantity = 14,
        ChangeBTPQuantities = 13,
        HandlingSuccess = 15,
        ClearData = 16
    }
}
