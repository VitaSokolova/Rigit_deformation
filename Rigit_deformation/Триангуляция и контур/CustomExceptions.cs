using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rigit_deformation.Триангуляция_и_контур
{
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    [Serializable]
    public class FirstPointNotFoundException : Exception
    {
        
        public FirstPointNotFoundException()
        {
        }

        public FirstPointNotFoundException(string message)
            : base(message)
        {
            MessageBox.Show(message);
        }

        public FirstPointNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected FirstPointNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
