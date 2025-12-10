SELECT TOP (1000)
    [ClubIDTienPVK]
      , [ClubCode]
      , [IsDeleted]
FROM [FA25_PRN222_3W_PRN222_01_G5_SCMS].[dbo].[ClubsTienPVK]

UPDATE [FA25_PRN222_3W_PRN222_01_G5_SCMS].[dbo].[ClubsTienPVK]
  SET IsDeleted = 0
  WHERE [IsDeleted] = 1
