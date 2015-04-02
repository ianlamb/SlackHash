using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SlackHash.Data;

namespace SlackHash.Game
{
	public class InputManager
	{
		const float analogLimit = 0.5f;

		static InputManager()
		{
			currentGamePadStates = new Dictionary<PlayerIndex, GamePadState>();
			currentGamePadStates.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
			currentGamePadStates.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
			currentGamePadStates.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
			currentGamePadStates.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));
		}

        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            ExitGame,
            MoveCharacterUp,
            MoveCharacterDown,
            MoveCharacterLeft,
            MoveCharacterRight,
			AttackWeaponOne,
			AttackWeaponTwo,
            Drop,
            Pickup,
            TotalActionCount,
        }

        public enum GamePadButtons
        {
            Start,
            Back,
            A,
            B,
            X,
            Y,
            Up,
            Down,
            Left,
            Right,
            LeftShoulder,
            RightShoulder,
            LeftTrigger,
            RightTrigger,
        }

		public enum MouseButtons
		{
			LeftButton,
			MiddleButton,
			RightButton
		}

        public class ActionMap
        {
            public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();
			public List<Keys> keyboardKeys = new List<Keys>();
			public List<MouseButtons> mouseButtons = new List<MouseButtons>();
        }

        #region Keyboard Data
		private static KeyboardState previousKeyboardState;
        private static KeyboardState currentKeyboardState;
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyTriggered(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)) &&
                (!previousKeyboardState.IsKeyDown(key));
        }
		#endregion


		#region Mouse Data
		private static MouseState previousMouseState;
		private static MouseState currentMouseState;
		public static MouseState CurrentMouseState
		{
			get { return currentMouseState; }
		}

		public static bool IsMouseLeftButtonPressed()
		{
			return (currentMouseState.LeftButton == ButtonState.Pressed);
		}

		public static bool IsMouseMiddleButtonPressed()
		{
			return (currentMouseState.MiddleButton == ButtonState.Pressed);
		}

		public static bool IsMouseRightButtonPressed()
		{
			return (currentMouseState.RightButton == ButtonState.Pressed);
		}

		private static bool IsMouseButtonPressed(MouseButtons mouseButton)
		{
			switch (mouseButton)
			{
				case MouseButtons.LeftButton:
					return IsMouseLeftButtonPressed();
				case MouseButtons.MiddleButton:
					return IsMouseMiddleButtonPressed();
				case MouseButtons.RightButton:
					return IsMouseRightButtonPressed();
			}

			return false;
		}

        public static bool IsMouseLeftButtonTriggered()
        {
            return ((currentMouseState.LeftButton == ButtonState.Pressed) &&
                (previousMouseState.LeftButton == ButtonState.Released));
        }

		public static bool IsMouseMiddleButtonTriggered()
		{
			return ((currentMouseState.MiddleButton == ButtonState.Pressed) &&
				(previousMouseState.MiddleButton == ButtonState.Released));
		}

		public static bool IsMouseRightButtonTriggered()
		{
			return ((currentMouseState.RightButton == ButtonState.Pressed) &&
				(previousMouseState.RightButton == ButtonState.Released));
		}

		private static bool IsMouseButtonTriggered(MouseButtons mouseButton)
		{
			switch (mouseButton)
			{
				case MouseButtons.LeftButton:
					return IsMouseLeftButtonTriggered();
				case MouseButtons.MiddleButton:
					return IsMouseMiddleButtonTriggered();
				case MouseButtons.RightButton:
					return IsMouseRightButtonTriggered();
			}

			return false;
		}

		#endregion


		#region GamePad Data
		private static Dictionary<PlayerIndex, GamePadState> previousGamePadStates;
		private static Dictionary<PlayerIndex, GamePadState> currentGamePadStates;
		public static Dictionary<PlayerIndex, GamePadState> CurrentGamePadStates
        {
			get { return currentGamePadStates; }
        }

        public static bool IsGamePadStartPressed(PlayerIndex pi)
        {
            return (currentGamePadStates[pi].Buttons.Start == ButtonState.Pressed);
        }

		public static bool IsGamePadBackPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.Back == ButtonState.Pressed);
        }

		public static bool IsGamePadAPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.A == ButtonState.Pressed);
        }

		public static bool IsGamePadBPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.B == ButtonState.Pressed);
        }

		public static bool IsGamePadXPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.X == ButtonState.Pressed);
        }

		public static bool IsGamePadYPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.Y == ButtonState.Pressed);
        }

		public static bool IsGamePadLeftShoulderPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.LeftShoulder == ButtonState.Pressed);
        }

		public static bool IsGamePadRightShoulderPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Buttons.RightShoulder == ButtonState.Pressed);
        }

		public static bool IsGamePadDPadUpPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].DPad.Up == ButtonState.Pressed);
        }

		public static bool IsGamePadDPadDownPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].DPad.Down == ButtonState.Pressed);
        }

		public static bool IsGamePadDPadLeftPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].DPad.Left == ButtonState.Pressed);
        }

		public static bool IsGamePadDPadRightPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].DPad.Right == ButtonState.Pressed);
        }

		public static bool IsGamePadLeftTriggerPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Triggers.Left > analogLimit);
        }

		public static bool IsGamePadRightTriggerPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].Triggers.Right > analogLimit);
        }

		public static bool IsGamePadLeftStickUpPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].ThumbSticks.Left.Y > analogLimit);
        }

		public static bool IsGamePadLeftStickDownPressed(PlayerIndex pi)
        {
			return (-1f * currentGamePadStates[pi].ThumbSticks.Left.Y > analogLimit);
        }

		public static bool IsGamePadLeftStickLeftPressed(PlayerIndex pi)
        {
			return (-1f * currentGamePadStates[pi].ThumbSticks.Left.X > analogLimit);
        }

		public static bool IsGamePadLeftStickRightPressed(PlayerIndex pi)
        {
			return (currentGamePadStates[pi].ThumbSticks.Left.X > analogLimit);
        }

		private static bool IsGamePadButtonPressed(GamePadButtons gamePadKey, PlayerIndex pi)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartPressed(pi);

                case GamePadButtons.Back:
					return IsGamePadBackPressed(pi);

                case GamePadButtons.A:
					return IsGamePadAPressed(pi);

                case GamePadButtons.B:
					return IsGamePadBPressed(pi);

                case GamePadButtons.X:
					return IsGamePadXPressed(pi);

                case GamePadButtons.Y:
					return IsGamePadYPressed(pi);

                case GamePadButtons.LeftShoulder:
					return IsGamePadLeftShoulderPressed(pi);

                case GamePadButtons.RightShoulder:
					return IsGamePadRightShoulderPressed(pi);

                case GamePadButtons.LeftTrigger:
					return IsGamePadLeftTriggerPressed(pi);

                case GamePadButtons.RightTrigger:
					return IsGamePadRightTriggerPressed(pi);

                case GamePadButtons.Up:
					return IsGamePadDPadUpPressed(pi) ||
						IsGamePadLeftStickUpPressed(pi);

                case GamePadButtons.Down:
					return IsGamePadDPadDownPressed(pi) ||
						IsGamePadLeftStickDownPressed(pi);

                case GamePadButtons.Left:
					return IsGamePadDPadLeftPressed(pi) ||
						IsGamePadLeftStickLeftPressed(pi);

                case GamePadButtons.Right:
					return IsGamePadDPadRightPressed(pi) ||
						IsGamePadLeftStickRightPressed(pi);
            }

            return false;
        }



		public static bool IsGamePadStartTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.Start == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.Start == ButtonState.Released));
        }

		public static bool IsGamePadBackTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.Back == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.Back == ButtonState.Released));
        }

		public static bool IsGamePadATriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.A == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.A == ButtonState.Released));
        }

		public static bool IsGamePadBTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.B == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.B == ButtonState.Released));
        }

		public static bool IsGamePadXTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.X == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.X == ButtonState.Released));
        }

		public static bool IsGamePadYTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Buttons.Y == ButtonState.Pressed) &&
              (previousGamePadStates[pi].Buttons.Y == ButtonState.Released));
        }

		public static bool IsGamePadLeftShoulderTriggered(PlayerIndex pi)
        {
            return (
                (currentGamePadStates[pi].Buttons.LeftShoulder == ButtonState.Pressed) &&
                (previousGamePadStates[pi].Buttons.LeftShoulder == ButtonState.Released));
        }

		public static bool IsGamePadRightShoulderTriggered(PlayerIndex pi)
        {
            return (
                (currentGamePadStates[pi].Buttons.RightShoulder == ButtonState.Pressed) &&
                (previousGamePadStates[pi].Buttons.RightShoulder == ButtonState.Released));
        }

		public static bool IsGamePadDPadUpTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].DPad.Up == ButtonState.Pressed) &&
              (previousGamePadStates[pi].DPad.Up == ButtonState.Released));
        }

		public static bool IsGamePadDPadDownTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].DPad.Down == ButtonState.Pressed) &&
              (previousGamePadStates[pi].DPad.Down == ButtonState.Released));
        }

		public static bool IsGamePadDPadLeftTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].DPad.Left == ButtonState.Pressed) &&
              (previousGamePadStates[pi].DPad.Left == ButtonState.Released));
        }

		public static bool IsGamePadDPadRightTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].DPad.Right == ButtonState.Pressed) &&
              (previousGamePadStates[pi].DPad.Right == ButtonState.Released));
        }

		public static bool IsGamePadLeftTriggerTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Triggers.Left > analogLimit) &&
                (previousGamePadStates[pi].Triggers.Left < analogLimit));
        }

		public static bool IsGamePadRightTriggerTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].Triggers.Right > analogLimit) &&
                (previousGamePadStates[pi].Triggers.Right < analogLimit));
        }

		public static bool IsGamePadLeftStickUpTriggered(PlayerIndex pi)
        {
            return ((currentGamePadStates[pi].ThumbSticks.Left.Y > analogLimit) &&
                (previousGamePadStates[pi].ThumbSticks.Left.Y < analogLimit));
        }

		public static bool IsGamePadLeftStickDownTriggered(PlayerIndex pi)
        {
			return ((-1f * currentGamePadStates[pi].ThumbSticks.Left.Y > analogLimit) &&
				(-1f * previousGamePadStates[pi].ThumbSticks.Left.Y < analogLimit));
        }

		public static bool IsGamePadLeftStickLeftTriggered(PlayerIndex pi)
        {
			return ((-1f * currentGamePadStates[pi].ThumbSticks.Left.X > analogLimit) &&
				(-1f * previousGamePadStates[pi].ThumbSticks.Left.X < analogLimit));
        }

		public static bool IsGamePadLeftStickRightTriggered(PlayerIndex pi)
        {
			return ((currentGamePadStates[pi].ThumbSticks.Left.X > analogLimit) &&
                (previousGamePadStates[pi].ThumbSticks.Left.X < analogLimit));
        }

		private static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey, PlayerIndex pi)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartTriggered(pi);

                case GamePadButtons.Back:
                    return IsGamePadBackTriggered(pi);

                case GamePadButtons.A:
                    return IsGamePadATriggered(pi);

                case GamePadButtons.B:
                    return IsGamePadBTriggered(pi);

                case GamePadButtons.X:
                    return IsGamePadXTriggered(pi);

                case GamePadButtons.Y:
                    return IsGamePadYTriggered(pi);

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderTriggered(pi);

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderTriggered(pi);

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerTriggered(pi);

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerTriggered(pi);

                case GamePadButtons.Up:
                    return IsGamePadDPadUpTriggered(pi) ||
                        IsGamePadLeftStickUpTriggered(pi);

                case GamePadButtons.Down:
                    return IsGamePadDPadDownTriggered(pi) ||
                        IsGamePadLeftStickDownTriggered(pi);

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftTriggered(pi) ||
                        IsGamePadLeftStickLeftTriggered(pi);

                case GamePadButtons.Right:
                    return IsGamePadDPadRightTriggered(pi) ||
                        IsGamePadLeftStickRightTriggered(pi);
            }

            return false;
        }
        #endregion


        #region Action Mapping
        private static ActionMap[] actionMaps;


        public static ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }

        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.MainMenu] = new ActionMap();
            actionMaps[(int)Action.MainMenu].keyboardKeys.Add(
                Keys.Tab);
            actionMaps[(int)Action.MainMenu].gamePadButtons.Add(
                GamePadButtons.Start);

            actionMaps[(int)Action.Ok] = new ActionMap();
            actionMaps[(int)Action.Ok].keyboardKeys.Add(
                Keys.Enter);
            actionMaps[(int)Action.Ok].gamePadButtons.Add(
                GamePadButtons.A);

            actionMaps[(int)Action.Back] = new ActionMap();
            actionMaps[(int)Action.Back].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.Back].gamePadButtons.Add(
                GamePadButtons.B);

            actionMaps[(int)Action.ExitGame] = new ActionMap();
            actionMaps[(int)Action.ExitGame].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.ExitGame].gamePadButtons.Add(
                GamePadButtons.Back);

            actionMaps[(int)Action.MoveCharacterUp] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterUp].keyboardKeys.Add(
                Keys.W);
            actionMaps[(int)Action.MoveCharacterUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.MoveCharacterDown] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterDown].keyboardKeys.Add(
                Keys.S);
            actionMaps[(int)Action.MoveCharacterDown].gamePadButtons.Add(
                GamePadButtons.Down);

            actionMaps[(int)Action.MoveCharacterLeft] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterLeft].keyboardKeys.Add(
                Keys.A);
            actionMaps[(int)Action.MoveCharacterLeft].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.MoveCharacterRight] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterRight].keyboardKeys.Add(
                Keys.D);
            actionMaps[(int)Action.MoveCharacterRight].gamePadButtons.Add(
                GamePadButtons.Right);

			actionMaps[(int)Action.Drop] = new ActionMap();
			actionMaps[(int)Action.Drop].keyboardKeys.Add(
				Keys.G);
			actionMaps[(int)Action.Drop].gamePadButtons.Add(
				GamePadButtons.Down);

			actionMaps[(int)Action.Pickup] = new ActionMap();
			actionMaps[(int)Action.Pickup].keyboardKeys.Add(
				Keys.F);
			actionMaps[(int)Action.Pickup].gamePadButtons.Add(
				GamePadButtons.Up);

			actionMaps[(int)Action.AttackWeaponOne] = new ActionMap();
			actionMaps[(int)Action.AttackWeaponOne].mouseButtons.Add(
				MouseButtons.LeftButton);
			actionMaps[(int)Action.AttackWeaponOne].gamePadButtons.Add(
				GamePadButtons.RightShoulder);

			actionMaps[(int)Action.AttackWeaponTwo] = new ActionMap();
			actionMaps[(int)Action.AttackWeaponTwo].mouseButtons.Add(
				MouseButtons.RightButton);
			actionMaps[(int)Action.AttackWeaponTwo].gamePadButtons.Add(
				GamePadButtons.LeftShoulder);
        }

        public static bool IsActionPressed(Action action, PlayerIndex pi)
        {
            return IsActionMapPressed(actionMaps[(int)action], pi);
        }

		public static bool IsActionTriggered(Action action, PlayerIndex pi)
        {
            return IsActionMapTriggered(actionMaps[(int)action], pi);
        }

		private static bool IsActionMapPressed(ActionMap actionMap, PlayerIndex pi)
		{
			if (pi == PlayerIndex.One)
			{
				for (int i = 0; i < actionMap.mouseButtons.Count; i++)
				{
					if (IsMouseButtonPressed(actionMap.mouseButtons[i]))
					{
						Logging.Info("Mouse Button Pressed: ", actionMap.mouseButtons[i]);
						return true;
					}
				}
				for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
				{
					if (IsKeyPressed(actionMap.keyboardKeys[i]))
					{
						Logging.Info("Key Pressed: ", actionMap.keyboardKeys[i]);
						return true;
					}
				}
			}
			if (currentGamePadStates[pi].IsConnected)
			{
				for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
				{
					if (IsGamePadButtonPressed(actionMap.gamePadButtons[i], pi))
					{
						Logging.Info("GamePad Button Pressed: ", actionMap.gamePadButtons[i]);
						return true;
					}
				}
			}
            return false;
        }

		private static bool IsActionMapTriggered(ActionMap actionMap, PlayerIndex pi)
		{
			if (pi == PlayerIndex.One)
			{
				for (int i = 0; i < actionMap.mouseButtons.Count; i++)
				{
					if (IsMouseButtonTriggered(actionMap.mouseButtons[i]))
					{
						Logging.Info("Mouse Button Triggered: ", actionMap.mouseButtons[i]);
						return true;
					}
				}
				for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
				{
					if (IsKeyTriggered(actionMap.keyboardKeys[i]))
					{
						Logging.Info("Key Triggered: ", actionMap.keyboardKeys[i]);
						return true;
					}
				}
			}
			if (currentGamePadStates[pi].IsConnected)
			{
				for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
				{
					if (IsGamePadButtonTriggered(actionMap.gamePadButtons[i], pi))
					{
						Logging.Info("GamePad Button Triggered: ", actionMap.gamePadButtons[i]);
						return true;
					}
				}
			}
            return false;
        }
        #endregion

        public static void Initialize()
        {
            ResetActionMaps();
        }

        public static void Update()
        {
            // update the keyboard state
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

			// update the mouse state
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

            // update the gamepad state
			previousGamePadStates = currentGamePadStates;
			for (var i = 0; i < currentGamePadStates.Keys.Count; i++)
			{
				currentGamePadStates[currentGamePadStates.Keys.ElementAt(i)] = 
					GamePad.GetState(currentGamePadStates.Keys.ElementAt(i));
			}
        }
	}
}
