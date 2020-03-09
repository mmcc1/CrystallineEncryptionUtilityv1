# Crystalline Encryption Utility v1.0

A basic implementation of the Crystalline Cipher employing the Lattice approach.  Lattice makes use of n number of Keys/Salts to encrypt a file.  Typically, each Key/Salt is used with 10 rounds (11 due to the code) then Key/Salts are switched and the process repeated.  In all, the basic settings use 110 rounds.

It is advisable to always go with odd settings for the rounds and file size of the Key/Salts, the more unique the better.

More instructions in the help file included.

Win10 security can block the files.  After downloading the zip archive, right-click and tick the 'Unblock' box.

Analysis of the Output without any compression:

!(https://github.com/mmcc1/CrystallineEncryptionUtilityv1/blob/master/output.tif)
