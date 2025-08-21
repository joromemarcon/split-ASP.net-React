-- Migration Script: Transfer data from 'jorometest' to 'jorometest@gmail.com'
-- This script safely transfers receipts and related data between users

-- Step 1: Check if both users exist
SELECT 'Checking Users' as Step;
SELECT Id, UserName, Email, FullName 
FROM AspNetUsers 
WHERE UserName IN ('jorometest', 'jorometest@gmail.com') 
   OR Email IN ('jorometest@gmail.com');

-- Step 2: Show current data owned by jorometest
SELECT 'Current Data for jorometest' as Step;
SELECT 
    u.UserName,
    cr.ReceiptId,
    r.ReceiptCode,
    r.EstablishmentName,
    r.TransactionTotal,
    cr.isOwner,
    cr.IsPaid
FROM AspNetUsers u
JOIN CustomerReceipts cr ON u.Id = cr.UserId
JOIN Receipts r ON cr.ReceiptId = r.Id
WHERE u.UserName = 'jorometest';

-- Step 3: Show items related to jorometest receipts
SELECT 'Items for jorometest receipts' as Step;
SELECT 
    i.Id as ItemId,
    i.ItemName,
    i.ItemPrice,
    i.PaidCustomerId,
    r.Id as ReceiptId,
    r.ReceiptCode
FROM Items i
JOIN Receipts r ON i.ReceiptId = r.Id
JOIN CustomerReceipts cr ON r.Id = cr.ReceiptId
JOIN AspNetUsers u ON cr.UserId = u.Id
WHERE u.UserName = 'jorometest';

-- Step 4: BEGIN TRANSACTION for safe migration
BEGIN TRANSACTION;

DECLARE @OldUserId NVARCHAR(450);
DECLARE @NewUserId NVARCHAR(450);

-- Get the user IDs
SELECT @OldUserId = Id FROM AspNetUsers WHERE UserName = 'jorometest';
SELECT @NewUserId = Id FROM AspNetUsers WHERE UserName = 'jorometest@gmail.com' OR Email = 'jorometest@gmail.com';

-- Check if both users exist
IF @OldUserId IS NULL
BEGIN
    PRINT 'Error: User jorometest not found';
    ROLLBACK TRANSACTION;
    RETURN;
END

IF @NewUserId IS NULL
BEGIN
    PRINT 'Error: User jorometest@gmail.com not found';
    ROLLBACK TRANSACTION;
    RETURN;
END

PRINT 'Old User ID: ' + @OldUserId;
PRINT 'New User ID: ' + @NewUserId;

-- Step 5: Update CustomerReceipts to point to new user
UPDATE CustomerReceipts 
SET UserId = @NewUserId
WHERE UserId = @OldUserId;

PRINT 'Updated ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' CustomerReceipt records';

-- Step 6: Update any PaidCustomerId references in Items table
-- (assuming PaidCustomerId should reference the user ID, though it's stored as int)
-- Note: This might need adjustment based on how PaidCustomerId is actually used
-- For now, we'll skip this as it appears to be an int, not a user reference

-- Step 7: Verify the migration
SELECT 'Verification - Data now belongs to new user' as Step;
SELECT 
    u.UserName,
    u.Email,
    cr.ReceiptId,
    r.ReceiptCode,
    r.EstablishmentName,
    r.TransactionTotal,
    cr.isOwner,
    cr.IsPaid
FROM AspNetUsers u
JOIN CustomerReceipts cr ON u.Id = cr.UserId
JOIN Receipts r ON cr.ReceiptId = r.Id
WHERE u.Id = @NewUserId;

-- Step 8: Show remaining data for old user (should be empty)
SELECT 'Verification - Old user should have no data' as Step;
SELECT 
    u.UserName,
    cr.ReceiptId,
    r.ReceiptCode
FROM AspNetUsers u
JOIN CustomerReceipts cr ON u.Id = cr.UserId
JOIN Receipts r ON cr.ReceiptId = r.Id
WHERE u.Id = @OldUserId;

-- Step 9: Commit if everything looks good
-- COMMIT TRANSACTION;
-- Uncomment the above line to actually commit the changes

-- For safety, we'll rollback for now so you can review the results
ROLLBACK TRANSACTION;
PRINT 'Transaction rolled back for safety. Review the results above.';
PRINT 'If everything looks correct, change ROLLBACK to COMMIT and run again.';