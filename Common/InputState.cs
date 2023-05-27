namespace ArrowGame.Common {
	public struct InputState {
		public float HorizontalMovement;

		public override string ToString() {
			return $"{nameof(HorizontalMovement)}: {HorizontalMovement}";
		}
	}
}
