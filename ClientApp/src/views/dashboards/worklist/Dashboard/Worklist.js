import * as React from 'react';
import WorklistTask from '../Dashboard/WorklistTask'
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';


export default function BoxSx() {
  return (
<>

    <Typography id="modal-modal-title" variant="h6" component="h2">
    WorkList
  </Typography>
    <Box
      sx={{
        width: 999,
        height: '30px',
        backgroundColor: 'primary.dark',
       
      }}
    />

    <WorklistTask/>







    </>
  );
}