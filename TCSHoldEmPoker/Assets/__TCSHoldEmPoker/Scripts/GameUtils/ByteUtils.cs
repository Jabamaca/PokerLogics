namespace GameUtils {
    public static class ByteUtils {
        public static byte ConvertBoolArrayToByte (bool[] source) {
            if (source.Length != 8)
                return 0;

            byte result = 0x00;
            int index = 0;
            foreach (bool b in source) {
                if (b)
                    result |= (byte)(0x01 << index);
                index++;
            }

            return result;
        }

        public static bool[] ConvertByteToBoolArray (byte b) {
            bool[] result = new bool[8];

            for (int i = 0; i < 8; i++)
                result[i] = (b & (0x01 << i)) != 0;

            return result;
        }
    }
}