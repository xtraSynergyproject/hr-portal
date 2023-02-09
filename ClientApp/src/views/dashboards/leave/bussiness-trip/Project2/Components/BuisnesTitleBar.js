import * as React from 'react';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import PersonIcon from '@mui/icons-material/Person';
import TagIcon from '@mui/icons-material/Tag';
import DoneAllIcon from '@mui/icons-material/DoneAll';
import CameraAltIcon from '@mui/icons-material/CameraAlt';

<<<<<<< HEAD
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon'







=======
>>>>>>> f52b79148e71e36001c1f597c65619bd12956f36
export default function MouseOverPopover() {
  const [anchorEl, setAnchorEl] = React.useState(null);

  const handlePopoverOpen = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handlePopoverClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);

<<<<<<< HEAD

  











=======
>>>>>>> f52b79148e71e36001c1f597c65619bd12956f36
  return (
    <div>
      <Grid container spacing={2}>
        <Grid item xs={3}>


          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >
<<<<<<< HEAD
             <Icon icon='mdi:close' />
          </IconButton>
    
=======
            <PersonIcon /> Administrator admin






          </Typography>

        </Grid>
        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >
            <TagIcon /> S-04-02-2023-3
>>>>>>> f52b79148e71e36001c1f597c65619bd12956f36





          </Typography>
        </Grid>


<<<<<<< HEAD
<Grid container spacing={2}>
  <Grid item xs={3}>

  
  <Typography
        aria-owns={open ? 'mouse-over-popover' : undefined}
        aria-haspopup="true"
        onMouseEnter={handlePopoverOpen}
        onMouseLeave={handlePopoverClose}
      >
      <PersonIcon/> Administrator admin


      


        
      </Typography>
    
  </Grid>
  <Grid item xs={3}>
  <Typography
        aria-owns={open ? 'mouse-over-popover' : undefined}
        aria-haspopup="true"
        onMouseEnter={handlePopoverOpen}
        onMouseLeave={handlePopoverClose}
      >
       <TagIcon/> S-04-02-2023-3
=======
        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >
            <DoneAllIcon />  Draft
>>>>>>> f52b79148e71e36001c1f597c65619bd12956f36





          </Typography>
        </Grid>


        <Grid item xs={3}>
          <Typography
            aria-owns={open ? 'mouse-over-popover' : undefined}
            aria-haspopup="true"
            onMouseEnter={handlePopoverOpen}
            onMouseLeave={handlePopoverClose}
          >
            <CameraAltIcon />  tomorrow
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