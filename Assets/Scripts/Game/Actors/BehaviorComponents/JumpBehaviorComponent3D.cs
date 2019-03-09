using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    public sealed class JumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
#region Actions
        public class JumpAction : CharacterBehaviorAction
        {
            public static JumpAction Default = new JumpAction();
        }
#endregion

        [SerializeField]
        private JumpBehaviorComponentData _data;

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpAction)) {
                return false;
            }

            if(!Behavior.IsGrounded || Behavior.IsSliding) {
                return false;
            }

            Behavior.Jump(_data.JumpHeight, _data.JumpParam);

            return true;
        }
    }
}
