using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    public interface FCSocketListener {
        void callBack(int socketID, int localSID, byte[] str, int len);
        void writeLog(int socketID, int localSID, int state, String log);
    }
}
