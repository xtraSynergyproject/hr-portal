import * as React from 'react'
import Pagination from '@mui/material/Pagination'
import Stack from '@mui/material/Stack'
// import * as React from 'react';
import Box from '@mui/material/Box'
import { Typography } from '@mui/material'

export default function PaginationButton() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'left', marginTop: '-20px' }}>
      <Box component='span' sx={{ p: 1, border: '1px solid' }}>
        <Box display={'flex'} alignItems={'center'}>
          <Stack
            spacing={4}
            paddingRight={80}
            // sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'left' }}
          >
            <Pagination count={10} showFirstButton showLastButton />
          </Stack>
          <Typography>No items to display</Typography>
        </Box>
      </Box>
    </Box>
  )
}