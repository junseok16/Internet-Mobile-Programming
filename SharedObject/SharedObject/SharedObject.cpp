#include "SharedObject.h"

#define LOGI(...) ((void)__android_log_print(ANDROID_LOG_INFO, "SharedObject3", __VA_ARGS__))
#define LOGW(...) ((void)__android_log_print(ANDROID_LOG_WARN, "SharedObject3", __VA_ARGS__))

extern "C" {
	// Static Variables
	// LCD
	static int lcd;
	static char msgs[2][20];

	// LED
	static int led;

	// 7 Segment
	static int seg;
	static int segnum;
	static int threadcond;
	static char nums[7];
	static pthread_t thd_seg;

	// Dot Matrix
	static int dot;
	static pthread_t thd_dot;
	static char temp[2];
	static char input[10];

	// Piezo
	static int piez;
	static pthread_t thd_piezo;

	// Keypad
	static int keypad;

	// Full Colored LED
	static int colorled;

	//////////////////////////////////////////////////

	int SharedObject::OPEN_LCD() {
		lcd = -1;
		lcd = open("/dev/fpga_textlcd", O_WRONLY);

		if (lcd < 0) {
			printf("ERROR: Fail to open LCD driver file.\n");
			return 1;
		}
		ioctl(lcd, TEXTLCD_INIT);
		return 0;
	}

	int SharedObject::CLOSE_LCD() {
		ioctl(lcd, TEXTLCD_OFF);
		close(lcd);
		return 0;
	}

	int SharedObject::CLEAR_LCD() {
		OPEN_LCD();
		ioctl(lcd, TEXTLCD_CLEAR);
		CLOSE_LCD();
		return 0;
	}

	int SharedObject::CTL_LCD(int stage) {
		OPEN_LCD();

		switch (stage) {
		case 1:
			strcpy(msgs[0], "Mercury Stage");
			strcpy(msgs[1], "Liberation Day");
			break;
		case 2:
			strcpy(msgs[0], "Earth Stage");
			strcpy(msgs[1], "The Blue Marble");
			break;
		case 3:
			strcpy(msgs[0], "Mars Stage");
			strcpy(msgs[1], "Shatter The Sky");
			break;
		case 4:
			strcpy(msgs[0], "Uranus Stage");
			strcpy(msgs[1], "All In");
			break;
		}

		ioctl(lcd, TEXTLCD_CLEAR);
		ioctl(lcd, TEXTLCD_LINE1);
		write(lcd, msgs[0], strlen(msgs[0]));
		ioctl(lcd, TEXTLCD_LINE2);
		write(lcd, msgs[1], strlen(msgs[1]));

		close(lcd);
		return 0;
	}

	//////////////////////////////////////////////////

	int SharedObject::STATE_LED(int num) {
		int ch = 0;
		switch (num) {
		case 0: ch = 0;
			break;
		case 1: ch = 1;
			break;
		case 2: ch = 3;
			break;
		case 3: ch = 7;
			break;
		case 4: ch = 15;
			break;
		case 5: ch = 31;
			break;
		case 6: ch = 63;
			break;
		case 7: ch = 127;
			break;
		case 8: ch = 255;
			break;
		default:
			break;
		}
		return ch;
	}

	int SharedObject::CTL_LED(int num) {
		led = -1;
		unsigned char ch;// LED from 0 to 255

		if (num > 8) {
			printf("ERROR: LED number should be from 0 to 8.\n");
			return 1;
		}
		led = open("/dev/fpga_led", O_WRONLY);
		if (led < 0) {
			printf("ERROR: Fail to open LED driver file.\n");
			return 1;
		}

		ch = STATE_LED(num);
		write(led, &ch, 1);
		close(led);
		return 0;
	}

	//////////////////////////////////////////////////

	int SharedObject::CTL_SEGMENT(int num) {
		if (threadcond == 0 && num != 999999) {
			threadcond = 1;
			pthread_create(&thd_seg, NULL, LOOP_SEGMENT, (void*)num);
		}
		if (num < 0 || num > 999999) {
			printf("ERROR: Segment should be 6 digits.\n");
			exit(1);
		}
		else {
			segnum = num;
		}
		return 0;
	}

	void* SharedObject::LOOP_SEGMENT(void* num) {
		pthread_detach(pthread_self());

		seg = -1;
		seg = open("/dev/fpga_segment", O_WRONLY);
		if (seg < 0) {
			printf("ERROR: Fail to open segment driver file.\n");
			exit(1);
		}
		sprintf(nums, "%06d", (int)num);

		while (1) {
			if (segnum == 999999) {
				threadcond = 0;
				close(seg);
				return NULL;
			}
			sprintf(nums, "%06d", segnum);
			write(seg, nums, 6);
			usleep(200);
		}
	}

	//////////////////////////////////////////////////

	int SharedObject::CTL_DOT(int num) {
		pthread_create(&thd_dot, NULL, THREAD_DOT, (void*)num);
		return 0;
	}

	void* SharedObject::THREAD_DOT(void* data) {
		pthread_detach(pthread_self());

		int i = 0;
		int j = 0;
		int offset = 0;
		int ch = 0;
		int len = 0;
		char* result = (char*)malloc(100);

		memset(result, '0', 100);

		dot = -1;
		dot = open("/dev/fpga_dotmatrix", O_WRONLY);

		if (dot < 0) {
			printf("ERROR : Fail to open dot_matrix driver file.\n");
			return NULL;
		}

		if ((int)data == 1) {
			strcpy(input, "YOU WIN!");
		}
		else if ((int)data == 2) {
			strcpy(input, "YOU LOSE");
		}
		else if ((int)data == 3) {
			strcpy(input, "CLEAR");
		}

		len = strlen(input);
		for (i = 0; i < len; i++) {
			ch = input[i];
			ch -= 0x20;

			for (j = 0; j < 5; j++) {
				sprintf(temp, "%x%x", font[ch][j] / 16, font[ch][j] % 16);

				result[offset++] = temp[0];
				result[offset++] = temp[1];
			}
			result[offset++] = '0';
			result[offset++] = '0';
		}

		for (i = 0; i < (offset - 18) / 2; i++) {
			for (j = 0; j < 20; j++) {
				write(dot, &result[2 * i], 20);
			}
		}
		close(dot);
		free(result);
		return NULL;
	}

	//////////////////////////////////////////////////

	int SharedObject::CTL_PIEZO(int music) {
		pthread_create(&thd_piezo, NULL, PLAY_PIEZO, (void*)music);
		return 0;
	}

	void* SharedObject::PLAY_PIEZO(void* song) {
		pthread_detach(pthread_self());
		piez = -1;
		piez = open("/dev/fpga_piezo", O_WRONLY);

		if (piez < 0) {
			printf("ERROR : Fail to open piezo driver file.\n");
			return NULL;
		}

		if ((int)song == 0) {
			BUTTON_CLICK();
		}
		else if ((int)song == 1) {
			VICTORY();
		}
		else if ((int)song == 2) {
			DEFEAT();
		}
		close(piez);
		return NULL;
	}

	/*
	* Note:
	*	d, D - do | r, R - re
	*	m, M - mi | f, F - fa
	*	s, S - so | l, L - la
	*	t, T - ti | v - void
	*
	* Duration Rate:
	*	100 msec by usleep(100000);
	*
	* Delay Rate:
	*	100 msec by usleep(100000);
	*/
	void SharedObject::MELODY(char note, float duration_rate, float delay_rate) {
		unsigned char nullValue = 0x00;
		unsigned char values[] = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27 };	// do, re, mi, ...
		int nnote;
		int duration = (int)(100000 * duration_rate);
		int delay = (int)(100000 * delay_rate);

		switch (note) {
		case 'd': nnote = 0;
			break;
		case 'r': nnote = 1;
			break;
		case 'm': nnote = 2;
			break;
		case 'f': nnote = 3;
			break;
		case 's': nnote = 4;
			break;
		case 'l': nnote = 5;
			break;
		case 't': nnote = 6;
			break;
		case 'D': nnote = 7;
			break;
		case 'R': nnote = 8;
			break;
		case 'M': nnote = 9;
			break;
		case 'F': nnote = 10;
			break;
		case 'S': nnote = 11;
			break;
		case 'L': nnote = 12;
			break;
		case 'T': nnote = 13;
			break;
		case 'v': nnote = 14;
			break;
		default:
			break;
		}

		if (nnote != 14) {
			write(piez, &values[nnote], 1);
			usleep(duration);
		}
		else {
			usleep(duration);
		}
		write(piez, &nullValue, 1);
		usleep(delay);
	}

	void SharedObject::BUTTON_CLICK() {
		MELODY('D', 1, 0);
	}

	void SharedObject::VICTORY() {
		MELODY('S', 6, 0);
		MELODY('R', 1, 0);
		MELODY('S', 1, 0);

		MELODY('L', 6, 0);
		MELODY('R', 1, 0);
		MELODY('L', 1, 0);

		MELODY('T', 6, 0);
		MELODY('R', 1, 0);
		MELODY('T', 1, 0);

		MELODY('d', 3, 0);
		MELODY('T', 3, 0);
		MELODY('L', 2, 0);

		MELODY('S', 3, 0);
	}

	void SharedObject::DEFEAT() {
		MELODY('F', 4, 0);
		MELODY('M', 2, 0);
		MELODY('R', 4, 0);
		MELODY('D', 2, 0);

		MELODY('R', 6, 0);
		MELODY('S', 6, 0);

		MELODY('D', 2, 0);
	}

	//////////////////////////////////////////////////

	int SharedObject::CTL_FLED(int num) {
		char value[3];

		switch (num) {
		case 1:
			value[0] = 100;// Red
			value[1] = 0;// Green
			value[2] = 0;// Blue
			break;
		case 2:
			value[0] = 0;// Red
			value[1] = 100;// Green
			value[2] = 0;// Blue
			break;
		case 3:
			value[0] = 0;// Red
			value[1] = 0;// Green
			value[2] = 100;// Blue
			break;
		case 4:
			value[0] = 100;// Red
			value[1] = 100;// Green
			value[2] = 100;// Blue
			break;
		case 5:
			value[0] = 0;// Red
			value[1] = 0;// Green
			value[2] = 0;// Blue
			break;
		}

		colorled = -1;
		colorled = open("/dev/fpga_fullcolorled", O_RDWR | O_SYNC);
		if (colorled < 0) {
			printf("ERROR: Fail to open fullcolorled driver file.\n");
			return 1;
		}

		ioctl(colorled, FULL_LED1);
		write(colorled, value, 3);

		ioctl(colorled, FULL_LED2);
		write(colorled, value, 3);

		ioctl(colorled, FULL_LED3);
		write(colorled, value, 3);

		ioctl(colorled, FULL_LED4);
		write(colorled, value, 3);

		close(colorled);
		return 0;
	}

	//////////////////////////////////////////////////

	// working area starts.
	int SharedObject::OPEN_KEYPAD() {
		keypad = -1;
		keypad = open("/dev/fpga_keypad", O_RDONLY);

		if (keypad < 0) {
			printf("ERROR: Fail to open keypad driver file.\n");
			return 1;
		}
		return 0;
	}

	char SharedObject::GET_KEYPAD() {
		char buffer[2];
		memset(buffer, 0, sizeof(buffer));
		// read(keypad, buffer, 1);
		read(keypad, buffer, 1);
		return buffer[0];
	}
	// working area ends.

	//////////////////////////////////////////////////
}