﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_HW2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nMake a selection:");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Quit");
            bool keepGoing = true;
            string selection = Console.ReadLine();

            while (keepGoing)
            {
                switch (selection)
                {
                    case "1":
                        //encryption happens here
                        Console.WriteLine("\n***Encryption***");
                        int[] eightBitInput = GetInput(8);
                        int[] tenBitKey = GetInput(10);
                        //you now have 8-bit input and key to work with

                        //Generate SDES keys
                        int[] firstKey = GenerateSDESKey(tenBitKey, "keyone");
                        int[] secondKey = GenerateSDESKey(tenBitKey, "keytwo");

                        Console.WriteLine("1st key");
                        PrintIntArray(firstKey);
                        Console.WriteLine("2nd key");
                        PrintIntArray(secondKey);

                        int[] initial_perm = IP(eightBitInput);

                        Console.WriteLine("After initial permutation::");
                        PrintIntArray(initial_perm);

                        int[] FK1 = FK(initial_perm, firstKey);

                        Console.WriteLine("After FK1::");
                        PrintIntArray(FK1);

                        int[] post_SW = SW(FK1);

                        Console.WriteLine("Post SW::");
                        PrintIntArray(post_SW);

                        int[] FK2 = FK(post_SW, secondKey);

                        Console.WriteLine("Post FK2::");
                        PrintIntArray(FK2);

                        int[] ciphertext = IPInverse(FK2);

                        Console.WriteLine("***CIPHERTEXT***::");
                        PrintIntArray(ciphertext);


                        Console.WriteLine("\nMake a selection:");
                        Console.WriteLine("1. Encrypt");
                        Console.WriteLine("2. Decrypt");
                        Console.WriteLine("3. Quit");
                        selection = Console.ReadLine();

                        break;

                    case "2":
                        //decryption happens here
                        //encryption happens here
                        Console.WriteLine("\n***Decryption***");
                        Console.WriteLine("Enter the 8-bit ciphertext as input");
                        int[] eightBitInput_d = GetInput(8);
                        int[] tenBitKey_d = GetInput(10);
                        //you now have 8-bit input and key to work with

                        //Generate SDES keys
                        int[] firstKey_d = GenerateSDESKey(tenBitKey_d, "keyone");
                        int[] secondKey_d = GenerateSDESKey(tenBitKey_d, "keytwo");

                        Console.WriteLine("1st key");
                        PrintIntArray(firstKey_d);
                        Console.WriteLine("2nd key");
                        PrintIntArray(secondKey_d);
                        
                        int[] initial_perm_d = IP(eightBitInput_d);

                        Console.WriteLine("After initial permutation::");
                        PrintIntArray(initial_perm_d);

                        int[] FK1_d = FK(initial_perm_d, secondKey_d);

                        Console.WriteLine("After FK1::");
                        PrintIntArray(FK1_d);

                        int[] post_SW_d = SW(FK1_d);

                        Console.WriteLine("Post SW::");
                        PrintIntArray(post_SW_d);

                        int[] FK2_d = FK(post_SW_d, firstKey_d);

                        Console.WriteLine("Post FK2::");
                        PrintIntArray(FK2_d);

                        int[] ciphertext_d = IPInverse(FK2_d);

                        Console.WriteLine("***CIPHERTEXT***::");
                        PrintIntArray(ciphertext_d);


                        Console.WriteLine("\nMake a selection:");
                        Console.WriteLine("1. Encrypt");
                        Console.WriteLine("2. Decrypt");
                        Console.WriteLine("3. Quit");
                        selection = Console.ReadLine();

                        break;

                    case "3":
                        keepGoing = false;
                        break;

                    default:
                        Console.WriteLine("Invalid, please enter number 1-3");
                        selection = Console.ReadLine();
                        break;


                }
            }
            //

            //byte[] x = {1, 0, 0, 0 ,0,0,0,1};
            //string s = "00000010";
            //byte[] y = StringToByte(s);

            //PrintByte(x);
            //PrintByte(y);


            //string key = "1010000010";
            //int[] keyArr = StringToKeyArr(key);

            //if keyArr elements returned contains all zeros
            // prompt user to re-enter 10-bit binary key
            // call StringToKeyArr() again

            //Console.Write("initial key array = ");
            //PrintIntArray(keyArr);

            //Generate SDES key
            //int[] keyone = GenerateSDESKey(keyArr,"keyone");
            //int[] keytwo = GenerateSDESKey(keyArr, "keytwo");

            //Console.ReadLine();
        }

        //This function prompts user for <REQUIREDLENGTH:INPUT>-bit input key, validates, and returns 8-bit byte array. If fails, returns an empty byte array of all 0's
        private static int[] GetInput(int requiredLength)
        {
            bool inputValid = false;
            string str = "";

            while (!inputValid)
            {
                if (requiredLength == 8)
                {
                    Console.WriteLine("Enter 8-bit input of 0's and 1's");
                }
                else if (requiredLength == 10)
                {
                    Console.WriteLine("Enter 10-bit input key of 0's and 1's");
                }
                else
                {
                    Console.WriteLine($"Enter {requiredLength}-bit input of 0's and 1's");
                }


                str = Console.ReadLine();

                //confirm string is only numeric
                int n;
                bool isNumeric = int.TryParse(str, out n);

                while (!isNumeric || str.Length != requiredLength)
                {
                    Console.WriteLine($"Can only enter numeric 0's and 1's, must be {requiredLength}-bits only");
                    Console.WriteLine("Re-enter input, or q to quit");
                    str = Console.ReadLine();
                    if (str == "q")
                        break;

                    isNumeric = int.TryParse(str, out n);
                }
                if (str == "q") break; //break if q was pressed


                if (requiredLength == 8)
                {
                    byte[] byteArray = StringToByte(str);

                    if (ValidateByteArray(byteArray))
                    {
                        return (StringToByteInt(str));
                    }
                    else
                    {
                        Console.WriteLine("Input invalid -- the string has all numeric characters but it must be only 0's and 1's, please retry");
                    }
                }
                else //10-bit key
                {
                    byte[] byteArray = StringToTenBit(str);

                    if (ValidateByteArray(byteArray))
                    {
                        return (StringToTenBitInt(str));
                    }
                    else
                    {
                        Console.WriteLine("Input invalid -- the string has all numeric characters but it must be only 0's and 1's, please retry");
                    }
                }



            }
            Console.WriteLine("Failure in GetInput, returning garbage array");

            int[] crapArray = { 0, 0, 0, 0, 0, 0, 0, 0 };
            return crapArray;
        }

        public static int[] GenerateSDESKey(int[] keyArr, string returnRequest)
        {

            // keyArr[] : 10-bit key
            //      [ 1][ 0][ 1][ 0][ 0][ 0][ 0][ 0][ 1][ 0]
            //      [k0][k1][k2][k3][k4][k5][k6][k7][k8][k9]
            // P10: [k2][k4][k1][k6][k3][k9][k0][k8][k7][k5]


            // Step 1: Permute P10
            //      [k0][k1][k2][k3][k4][k5][k6][k7][k8][k9]
            // P10: [k2][k4][k1][k6][k3][k9][k0][k8][k7][k5]
            //      [ 1][ 0][ 0][ 0][ 0][ 0][ 1][ 1][ 0][ 0]
            int[] newKeyArr = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            newKeyArr[0] = keyArr[2];
            newKeyArr[1] = keyArr[4];
            newKeyArr[2] = keyArr[1];
            newKeyArr[3] = keyArr[6];
            newKeyArr[4] = keyArr[3];
            newKeyArr[5] = keyArr[9];
            newKeyArr[6] = keyArr[0];
            newKeyArr[7] = keyArr[8];
            newKeyArr[8] = keyArr[7];
            newKeyArr[9] = keyArr[5];

            //PrintIntArray(newKeyArr);

            // Step 2: Circular left shift LS-1
            //      [ 1][ 0][ 0][ 0][ 0]    [ 0][ 1][ 1][ 0][ 0]
            //      [k0][k1][k2][k3][k4]    [k5][k6][k7][k8][k9]
            //      [ 0][ 0][ 0][ 0][ 1]    [ 1][ 1][ 0][ 0][ 0]
            int shift = 1;
            int[] firstHalf = new int[] { 0, 0, 0, 0, 0 };
            int[] secondHalf = new int[] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < (newKeyArr.Length / 2); i++)
            {
                firstHalf[i] = newKeyArr[i];
                secondHalf[i] = newKeyArr[5 + i];
            }

            for (int i = 0; i < (newKeyArr.Length / 2); i++)
            {
                newKeyArr[i] = firstHalf[(i + shift + 5) % 5];
                newKeyArr[5 + i] = secondHalf[(i + shift + 5) % 5];
            }

            // Step 3: Permute P8
            //      [ 0][ 0][ 0][ 0][ 1][ 1][ 1][ 0][ 0][ 0]
            //      [k0][k1][k2][k3][k4][k5][k6][k7][k8][k9]
            // Subkey 1:
            //      [k5][k2][k6][k3][k7][k4][k9][k8]
            //      [ 1][ 0][ 1][ 0][ 0][ 1][ 0][ 0]
            int[] firstSubkey = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            firstSubkey[0] = newKeyArr[5];
            firstSubkey[1] = newKeyArr[2];
            firstSubkey[2] = newKeyArr[6];
            firstSubkey[3] = newKeyArr[3];
            firstSubkey[4] = newKeyArr[7];
            firstSubkey[5] = newKeyArr[4];
            firstSubkey[6] = newKeyArr[9];
            firstSubkey[7] = newKeyArr[8];

            if (returnRequest == "keyone")
            {
                return firstSubkey;
            }

            // Step 4: Circular left shift LS-2
            //      [ 0][ 0][ 0][ 0][ 1]    [ 1][ 1][ 0][ 0][ 0]
            //      [k0][k1][k2][k3][k4]    [k5][k6][k7][k8][k9]
            //      [ 0][ 0][ 1][ 0][ 0]    [ 0][ 0][ 0][ 1][ 1]
            shift = 2;
            for (int i = 0; i < (newKeyArr.Length / 2); i++)
            {
                firstHalf[i] = newKeyArr[i];
                secondHalf[i] = newKeyArr[5 + i];
            }
            for (int i = 0; i < (newKeyArr.Length / 2); i++)
            {
                newKeyArr[i] = firstHalf[(i + shift + 5) % 5];
                newKeyArr[5 + i] = secondHalf[(i + shift + 5) % 5];
            }

            // Step 5: Permute P8
            //      [ 0][ 0][ 1][ 0][ 0][ 0][ 0][ 0][ 1][ 1]
            //      [k0][k1][k2][k3][k4][k5][k6][k7][k8][k9]
            // Subkey 2:
            //      [k5][k2][k6][k3][k7][k4][k9][k8]
            //      [ 0][ 1][ 0][ 0][ 0][ 0][ 1][ 1]
            int[] secondSubkey = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            secondSubkey[0] = newKeyArr[5];
            secondSubkey[1] = newKeyArr[2];
            secondSubkey[2] = newKeyArr[6];
            secondSubkey[3] = newKeyArr[3];
            secondSubkey[4] = newKeyArr[7];
            secondSubkey[5] = newKeyArr[4];
            secondSubkey[6] = newKeyArr[9];
            secondSubkey[7] = newKeyArr[8];

            if (returnRequest == "keytwo")
                return secondSubkey;
            else
            {
                Console.WriteLine("Not returning output in GenerateDESKey, returning empty int array here");
                return new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        //IP - InitialPermutation
        //INPUT: 1 int array, 8-bits long
        //OUTPUT: a Permuted 8-bit int array
        //Intial: [0][1][2][3][4][5][6][7]
        //Becomes:[1][5][2][0][3][7][4][6]
        public static int[] IP(int[] inputArray)
        {
            int[] outputArray = { 0, 0, 0, 0, 0, 0, 0, 0 };

            outputArray[0] = inputArray[3];
            outputArray[1] = inputArray[0];
            outputArray[2] = inputArray[2];
            outputArray[3] = inputArray[4];
            outputArray[4] = inputArray[6];
            outputArray[5] = inputArray[1];
            outputArray[6] = inputArray[7];
            outputArray[7] = inputArray[5];

            return outputArray;
        }

        //IPInverse - InitialPermutation^-1
        //INPUT: 1 int array, 8-bits long
        //OUTPUT: a Permuted 8-bit int array
        //Intial: [0][1][2][3][4][5][6][7]
        //Becomes:[3][0][2][4][6][1][7][5]
        public static int[] IPInverse(int[] inputArray)
        {
            int[] outputArray = { 0, 0, 0, 0, 0, 0, 0, 0 };

            outputArray[0] = inputArray[1];
            outputArray[1] = inputArray[5];
            outputArray[2] = inputArray[2];
            outputArray[3] = inputArray[0];
            outputArray[4] = inputArray[3];
            outputArray[5] = inputArray[7];
            outputArray[6] = inputArray[4];
            outputArray[7] = inputArray[6];

            return outputArray;
        }

        //SW - Switch
        //INPUT: 1 int array, 8-bits long
        //OUTPUT: a Permuted 8-bit int array
        //Intial: [0][1][2][3][4][5][6][7]
        //Becomes:[4][5][6][7][0][1][2][3]
        public static int[] SW(int[] inputArray)
        {
            int[] outputArray = { 0, 0, 0, 0, 0, 0, 0, 0 };

            outputArray[0] = inputArray[4];
            outputArray[1] = inputArray[5];
            outputArray[2] = inputArray[6];
            outputArray[3] = inputArray[7];
            outputArray[4] = inputArray[0];
            outputArray[5] = inputArray[1];
            outputArray[6] = inputArray[2];
            outputArray[7] = inputArray[3];

            return outputArray;
        }

        //accepts 2 4-bit arrays, returns 4-bit array with XOR result
        public static int[] XOR(int[] top, int[] bot)
        {
            int[] outputArray = { 0, 0, 0, 0};

            if (top.Length == 8 && bot.Length == 8)
            {
                //handle 8-bit XOR
                int[] eightBitOutputArray = {0,0,0,0,0,0,0,0};
                eightBitOutputArray[0] = top[0] ^ bot[0];
                eightBitOutputArray[1] = top[1] ^ bot[1];
                eightBitOutputArray[2] = top[2] ^ bot[2];
                eightBitOutputArray[3] = top[3] ^ bot[3];
                eightBitOutputArray[4] = top[4] ^ bot[4];
                eightBitOutputArray[5] = top[5] ^ bot[5];
                eightBitOutputArray[6] = top[6] ^ bot[6];
                eightBitOutputArray[7] = top[7] ^ bot[7];


                return eightBitOutputArray;
            }
            else if (top.Length != 4 || bot.Length != 4)
            {
                Console.WriteLine("ERROR in XOR, top or bottom int array is not of length 4, returning empty output array");
                return outputArray; //return empty output array
            }

            outputArray[0] = top[0] ^ bot[0];
            outputArray[1] = top[1] ^ bot[1];
            outputArray[2] = top[2] ^ bot[2];
            outputArray[3] = top[3] ^ bot[3];
            return outputArray;
        }

        //FK function
        //takes 8-bit array for input and 8-bit key, returns 8-bits
        public static int[] FK(int[] input, int[] key)
        {
            int[] outputArray = { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (input.Length != 8)
            {
                Console.WriteLine("Error in FK - input array is not 8-bits long, returning empty array");
                return outputArray;
            }

            int[] lhs = { input[0], input[1], input[2], input[3] };
            int[] rhs = { input[4], input[5], input[6], input[7] };

            //rhs goes through EP
            int[] eight_bit_ep_result = EP(rhs);

            int[] post_ep_xor = XOR(eight_bit_ep_result, key);

            int[] post_ep_lhs = {post_ep_xor[0], post_ep_xor[1], post_ep_xor[2], post_ep_xor[3]};
            int[] post_ep_rhs = { post_ep_xor[4], post_ep_xor[5], post_ep_xor[6], post_ep_xor[7] };


            int[] s0_result = S0(post_ep_lhs);
            int[] s1_result = S1(post_ep_rhs);

            //4-bits int array fed into P4
            int[] preP4array = CombineTwoBitIntArray(s0_result, s1_result);

            //4-bit result from P4
            int[] postP4array = P4(preP4array);

            int[] result_lhs = XOR(lhs, postP4array);
            //rhs gets passed through untouched
            int[] result_rhs = rhs;

            //combine resulting LHS and RHS right before end of FK
            int[] realOutputArray = CombineFourBitIntArray(result_lhs, result_rhs);

            return realOutputArray;
        }

        //E/P, expansion/permutation, expands 4-bit array into 8-bit array
        // [0][1][2][3] INPUT
        // [3][0][1][2][1][2][3][1] OUTPUT
        public static int[] EP(int[] input)
        {
            int[] outputArray = { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (input.Length != 4)
            {
                Console.WriteLine("ERROR in E/P, input array is not of length 4, returning empty output array");
                return outputArray; //return empty output array
            }

            outputArray[0] = input[3];
            outputArray[1] = input[0];
            outputArray[2] = input[1];
            outputArray[3] = input[2];
            outputArray[4] = input[1];
            outputArray[5] = input[2];
            outputArray[6] = input[3];
            outputArray[7] = input[1];

            return outputArray;
        }

        //performs S0 table lookup, turns 4 bits into 2
        public static int[] S0(int[] input)
        {
            int[,] s0 = new int[,]
            {
                {1,0,3,2},
                {3,2,1,0},
                {0,2,1,3},
                {3,1,3,2}
            };

            if (input.Length != 4)
            {
                Console.WriteLine("ERROR IN S0 - input length != 4");
                return (new int[]{ 0,0,0,0});
            }

            int rowval = GetBinary(input[0],input[3]);
            int colval = GetBinary(input[1],input[2]);

            int x = s0[rowval,colval];
            int[] outputArray = GetInt(x);

            //2 bit int array is return
            return outputArray;
        }

        //performs S0 table lookup, turns 4 bits into 2
        public static int[] S1(int[] input)
        {
            int[,] s1 = new int[,]
            {
                {0,1,2,3},
                {2,0,1,3},
                {3,0,1,0},
                {2,1,0,3}
            };

            if (input.Length != 4)
            {
                Console.WriteLine("ERROR IN S1 - input length != 4");
                return (new int[] { 0, 0, 0, 0 });
            }

            int rowval = GetBinary(input[0], input[3]);
            int colval = GetBinary(input[1], input[2]);

            int x = s1[rowval, colval];
            int[] outputArray = GetInt(x);

            //2 bit int array is return
            return outputArray;
        }

        //permutation
        //INPUT [0][1][2][3]
        //OUTPUT[1][3][2][0]
        public static int[] P4(int[] input)
        {
            int[] outputArray = { input[1], input[3], input[2], input[0]};
            return outputArray;
        }

        //given an int, returns a 2-bit array of the binary result
        public static int[] GetInt(int num)
        {
            int[] outputArray = { 0, 0 };

            if (num == 0)
            {
                return outputArray;
            }
            if (num == 1)
            {
                outputArray[1] = 1;
                return outputArray;
            }
            if (num == 2)
            {
                outputArray[0] = 1;
                return outputArray;
            }
            if (num == 3)
            {
                outputArray[0] = 1;
                outputArray[1] = 1;
                return outputArray;
            }

            Console.WriteLine("Error in GetInt, input num > 3 OR < 0, returning 0.");
            return outputArray;
        }

        //given two ints that are assumed to be only 0's and 1's, returns the resulting binary number
        public static int GetBinary(int first, int second)
        {
            if (first == 0 && second == 0)
            {
                return 0;
            }
            else if (first == 0 && second == 1)
            {
                return 1;
            }
            else if (first == 1 && second == 0)
            {
                return 2;
            }
            else if (first == 1 && second == 1)
            {
                return 3;
            }
            else
            {
                Console.WriteLine("INVALID INPUT in GetBinary, returning 0");
                return 0;
            }
        }

        //adds 2 2-element int arrays together into 1 4-element int array
        public static int[] CombineTwoBitIntArray(int[] first, int[] second)
        {
            int[] outputArray = {first[0], first[1],second[0],second[1] };

            return outputArray;
        }

        //adds 2 4-element int arrays together into 1 8-element int array
        public static int[] CombineFourBitIntArray(int[] first, int[] second)
        {
            int[] outputArray = {first[0],first[1],first[2],first[3],second[0],second[1],second[2],second[3]};

            return outputArray;
        }

        //Byte & String Manipulation functions
        //ByteToString - converts byteArray to string
        public static string ByteToString(byte[] byteArray)
        {
            if (ValidateByteArray(byteArray))
            {
                return System.Text.Encoding.UTF8.GetString(byteArray);
            }
            else
            {
                Console.WriteLine("Invalid bytearray in ByteToString");
                return "NULLSTR";
            }
        }

        //StringToKeyArr - converts string key to int array, 10 bits long
        public static int[] StringToKeyArr(string key)
        {
            int[] keyArr = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (key.Length == 10)
            {
                for (int i = 0; i < key.Length; ++i)
                {
                    if (!key[i].Equals('0') && !key[i].Equals('1'))
                    {
                        Console.WriteLine("non 0-1 char passed into str, Error, returning empty Array");
                        return null;
                    }
                    else
                    {
                        if (key[i] == '1')
                        {
                            keyArr[i] = 1;
                        }
                        else
                        {
                            keyArr[i] = 0;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Key must be 10 bits long. Returning key of [0000000000]!\n");
            }

            return keyArr;
        }

        //prints a ByteArray out that is either 8 or 10-bits long
        public static void PrintByte(byte[] byteArray)
        {
            if (byteArray.Length == 8)
            {
                if (ValidateByteArray(byteArray) == true)
                {
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}\n", byteArray[0], byteArray[1], byteArray[2], byteArray[3], byteArray[4], byteArray[5], byteArray[6], byteArray[7]);
                }
                else
                {
                    Console.Write("There was an error in validating byte array ");
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}\n", byteArray[0], byteArray[1], byteArray[2], byteArray[3], byteArray[4], byteArray[5], byteArray[6], byteArray[7]);
                }
            }
            else if (byteArray.Length == 10)
            {
                if (ValidateByteArray(byteArray) == true)
                {
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n", byteArray[0], byteArray[1], byteArray[2], byteArray[3], byteArray[4], byteArray[5], byteArray[6], byteArray[7], byteArray[8], byteArray[9]);
                }
                else
                {
                    Console.Write("There was an error in validating byte array ");
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n", byteArray[0], byteArray[1], byteArray[2], byteArray[3], byteArray[4], byteArray[5], byteArray[6], byteArray[7], byteArray[8], byteArray[9]);
                }
            }
            else
            {
                Console.WriteLine("Invalid Byte Length, must be 8 or 10 bit long");
            }
        }

        //Prints an Int array of 10-elements/bits (for key)
        public static void PrintIntArray(int[] intArr)
        {
            if (intArr.Length == 10)
            {
                if (CheckBinaryArr(intArr) == true)
                {
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n", intArr[0], intArr[1], intArr[2], intArr[3], intArr[4], intArr[5], intArr[6], intArr[7], intArr[8], intArr[9]);
                }
                else
                {
                    Console.Write("There was an error in validating byte array ");
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n", intArr[0], intArr[1], intArr[2], intArr[3], intArr[4], intArr[5], intArr[6], intArr[7], intArr[8], intArr[9]);
                }
            }
            else if (intArr.Length == 8)
            {
                if (CheckBinaryArr(intArr) == true)
                {
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}\n", intArr[0], intArr[1], intArr[2], intArr[3], intArr[4], intArr[5], intArr[6], intArr[7]);
                }
                else
                {
                    Console.Write("There was an error in validating byte array ");
                    Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}\n", intArr[0], intArr[1], intArr[2], intArr[3], intArr[4], intArr[5], intArr[6], intArr[7]);
                }
            }
            else
            {
                Console.WriteLine("Invalid Byte Length, must be 10 bit long");
            }
        }

        //Checks if the Int array contains only 0's and 1's
        public static bool CheckBinaryArr(int[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i] != 1 && array[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //confirms that a byteArray contains only 0's and 1's
        public static bool ValidateByteArray(byte[] byteArray)
        {
            for (int i = 0; i < byteArray.Length; ++i)
            {
                if (byteArray[i] != 1 && byteArray[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //StringToByte - converts byte array, 8 bits long, to a string
        public static byte[] StringToByte(string str)
        {
            byte[] byteArray = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int i = 0; i < str.Length; ++i)
            {
                if (!str[i].Equals('0') && !str[i].Equals('1'))
                {
                    Console.WriteLine("non 0-1 char passed into str, Error, returning empty ByteArray");
                    return byteArray;
                }
                else
                {
                    if (str[i] == '1')
                    {
                        byteArray[i] = 1;
                    }
                    else
                    {
                        byteArray[i] = 0;
                    }
                }
            }

            return byteArray;
        }

        //StringToByte - converts byte array, 10 bits long, to a string
        public static byte[] StringToTenBit(string str)
        {
            byte[] byteArray = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int i = 0; i < str.Length; ++i)
            {
                if (!str[i].Equals('0') && !str[i].Equals('1'))
                {
                    Console.WriteLine("non 0-1 char passed into str, Error, returning empty ByteArray");
                    return byteArray;
                }
                else
                {
                    if (str[i] == '1')
                    {
                        byteArray[i] = 1;
                    }
                    else
                    {
                        byteArray[i] = 0;
                    }
                }
            }

            return byteArray;
        }

        //StringToByte - converts str to 8 bit int array
        public static int[] StringToByteInt(string str)
        {
            int[] intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int i = 0; i < str.Length; ++i)
            {
                if (!str[i].Equals('0') && !str[i].Equals('1'))
                {
                    Console.WriteLine("non 0-1 char passed into str, Error, returning empty intArray");
                    return intArray;
                }
                else
                {
                    if (str[i] == '1')
                    {
                        intArray[i] = 1;
                    }
                    else
                    {
                        intArray[i] = 0;
                    }
                }
            }

            return intArray;
        }

        //StringToByte - converts byte array, 10 bits long, to a string
        public static int[] StringToTenBitInt(string str)
        {
            int[] intArray = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int i = 0; i < str.Length; ++i)
            {
                if (!str[i].Equals('0') && !str[i].Equals('1'))
                {
                    Console.WriteLine("non 0-1 char passed into str, Error, returning empty intArray");
                    return intArray;
                }
                else
                {
                    if (str[i] == '1')
                    {
                        intArray[i] = 1;
                    }
                    else
                    {
                        intArray[i] = 0;
                    }
                }
            }

            return intArray;
        }
    }
}
