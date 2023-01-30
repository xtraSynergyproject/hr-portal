import * as React from 'react';
import Button from '@mui/material/Button';
import ButtonGroup from '@mui/material/ButtonGroup';
import Box from '@mui/material/Box';
import FilePresentIcon from '@mui/icons-material/FilePresent';
import CloseIcon from '@mui/icons-material/Close';
import SellIcon from '@mui/icons-material/Sell';

export default function NehaPagal() {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        '& > *': {
          m: 1,
        },
      }}
    >
      
      <ButtonGroup variant="text" aria-label="text button group">
      <Button> Close<CloseIcon/></Button>
      <Button> N-26-12</Button>
      <Button>Draft</Button>
      <Button> version
        <p>Version No.</p>
      </Button>
        <Button>
          <p>0</p>Attachment<FilePresentIcon/></Button>
        <Button>Tags<SellIcon/></Button>
        <Button>Tags<SellIcon/></Button>
        {/* <Button>Tags<SellIcon/></Button>
        <Button>Tags<SellIcon/></Button> */}
      </ButtonGroup>
    </Box>
  );
}
