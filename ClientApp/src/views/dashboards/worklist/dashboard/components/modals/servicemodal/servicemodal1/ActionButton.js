import * as React from 'react';
import {Box,Button} from '@mui/material';
import Fab from '@mui/material/Fab';
import EditIcon from '@mui/icons-material/Edit';
import { useState } from 'react';
import { AttachFile } from '@mui/icons-material';
import CloseIcon from '@mui/icons-material/Close';
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import EmailIcon from '@mui/icons-material/Email';
import { Card, Divider } from '@mui/material'


export default function ActionButton() {
  const [open, setOpen] = React.useState(false)
  // const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <Box sx={{ '& > :not(style)': { mt:6} }}>
      <Fab size="small" color="primary" aria-label="edit">
        <EditIcon />
      </Fab>
      <Fab size="small"  color="secondary" aria-label="attach">
        <AttachFile />
      </Fab>

      <Fab size="small"  color="secondary" aria-label="email">
        <EmailIcon />
      </Fab>

      <Fab size="small" disabled aria-label="more">
        <MoreHorizIcon />
     </Fab>
   {/*} <Button onClick={handleClose}>      
     <Fab size="small" color="secondary" aria-label="close" > 
        <CloseIcon />
       </Fab> 
      </Button>  */}
 
 
    </Box>
  );
}