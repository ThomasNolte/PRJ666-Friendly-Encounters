using UnityEngine.Networking;

public class MyMsgType
{
    public static short ChatMsg = MsgType.Highest + 1;
    public static short JoinMsg = MsgType.Highest + 2;
    public static short KickMsg = MsgType.Highest + 3;
}
