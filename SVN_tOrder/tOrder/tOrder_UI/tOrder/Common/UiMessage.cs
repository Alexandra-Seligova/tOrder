//===================================================================
// $Workfile:: UiMessage.cs
// $Author:: Alexandra Seligová
// $Revision:: 1
// $Date:: 2025-06-05
//===================================================================
// Description: SPC - tOrder
//     Generic UI Message for ViewModel-to-View communication
//===================================================================

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace tOrder.Common
{
    /// <summary>
    /// Simple UI message with a string payload.
    /// Used for signaling UI-level actions like "ToggleMenu".
    /// </summary>
    public sealed class UiMessage : ValueChangedMessage<string>
    {
        public UiMessage(string value) : base(value)
        {
        }
    }
}
