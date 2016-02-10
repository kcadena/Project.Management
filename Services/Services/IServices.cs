using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        void enviarCorreo(string emaildes, string mensaje);
    }
}
