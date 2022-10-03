#pragma once

extern "C" {
	namespace SharedObject {

		/*
		*	Hardware: Text LCD
		*	Menual: CTL_LCD(int stage); (stage from 1 to 4)
		*/
		int OPEN_LCD();
		int CLOSE_LCD();
		int CLEAR_LCD();
		int CTL_LCD(int stage);

		/*
		*	Hardware: LED
		*	Menual:	CTL_LED(int num); (num FROM 0 TO 8)
		*/
		int STATE_LED(int num);
		int CTL_LED(int num);

		/*
		* 	Hardware: 7 Segment
		*	Menual: CTL_SEGMENT(int num); (num FROM 0 TO 999999)
		*/
		int CTL_SEGMENT(int num);
		void* LOOP_SEGMENT(void* num);

		/*
		*	Hardware: Dot Matrix
		*	Menual: CTL_DOT(int num);
		*/
		int CTL_DOT(int num);
		void* THREAD_DOT(void* data);

		/*
		*	Hardware: Piezo
		*	Menual: CTL_PIEZO(int music);
		*	Music list: 1) Button (OnClick), 2) Victory(You Win), 3) Defeat(You Lose)
		*/
		int CTL_PIEZO(int music);
		void* PLAY_PIEZO(void* song);
		void MELODY(char note, float duration_rate, float delay_rate);

		void BUTTON_CLICK();
		void VICTORY();
		void DEFEAT();

		/*
		* 	Hardware: Full Colored LED
		*	Menual: CTL_FLED();
		*/
		int CTL_FLED(int num);

		/*
		* 	Hardware: Keypad
		*	Menual:
		*/
		int OPEN_KEYPAD();
		char GET_KEYPAD();
	}
}