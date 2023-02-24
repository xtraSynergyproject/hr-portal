import * as React from 'react';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import ProfileCardHeader from 'src/views/dashboards/hr-dashboard/home/ProfileCardHeader';
import AdministratorCard from 'src/views/dashboards/hr-dashboard/home/AdministratorCard';
import AdministratorCommentsCard from 'src/views/dashboards/hr-dashboard/home/AdministratorCommentsCard';
import AnnouncementCard from 'src/views/dashboards/hr-dashboard/home/AnnouncementCard';
import CommentsCard from 'src/views/dashboards/hr-dashboard/home/CommentsCard';


const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
}));

export default function Home() {
  return (
    <Box sx={{ flexGrow: 1 }}>
      <Grid container spacing={2} sx={{m:2}}>
        <Grid item xs={12} sx={{m:2}}>
          <Item>
            <ProfileCardHeader />
          </Item>
        </Grid>

          <Grid item xs={12} sx={{mt:2}}>
            <Item><AnnouncementCard/></Item>
          </Grid>


          <Grid item xs={5} sx={{mt:2}}>
            <Item><AdministratorCard/></Item>
          </Grid>
        

          <Grid item xs={7} sx={{mt:2}}>
            <Item><AdministratorCommentsCard/></Item>
          </Grid>
        </Grid>
    </Box>
  );
}
