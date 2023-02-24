import * as React from 'react';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import PersonIcon from '@mui/icons-material/Person';
import TagIcon from '@mui/icons-material/Tag';
import Box from '@mui/material/Box'
import DoneAllIcon from '@mui/icons-material/DoneAll';
import CameraAltIcon from '@mui/icons-material/CameraAlt';

import EditIcon from '@mui/icons-material/Edit';
import AttachFileIcon from '@mui/icons-material/AttachFile';
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import CancelIcon from '@mui/icons-material/Cancel';

export default function MouseOverPopover() {
  const [anchorEl, setAnchorEl] = React.useState(null);

  const handlePopoverOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);

  return (
    <div>
      <Grid sx={{ float: 'right', margin: '2px' }}>
        <EditIcon />
        <AttachFileIcon />
        <MoreHorizIcon />
        <CancelIcon />
      </Grid>

      <Grid container spacing={2}>
        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >

            <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
              <PersonIcon />

              <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                
                Administrator admin
              </Typography>
            </Box>
          </Typography>
        </Grid>
        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >

            <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
              <TagIcon />

              <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                {/* Joined {data.joiningDate} */}
                S-04-02-2023-3
              </Typography>
            </Box>
          </Typography>
        </Grid>


        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >

            <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
              <DoneAllIcon />

              <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                {/* Joined {data.joiningDate} */}
                Draft
              </Typography>
            </Box>

          </Typography>
        </Grid>


        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >

            <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
              <CameraAltIcon />

              <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                {/* Joined {data.joiningDate} */}
                tomorrow
              </Typography>
            </Box>


          </Typography>

        </Grid>

      </Grid>

      <Popover
        id="mouse-over-popover"
        sx={{
          pointerEvents: 'none',
        }}
        open={open}
        anchorEl={anchorEl}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        onClose={handlePopoverClose}
        disableRestoreFocus
      >
        <Typography sx={{ p: 1 }}>Service Owner/Requester</Typography>

      </Popover>
    </div>
  );
}
