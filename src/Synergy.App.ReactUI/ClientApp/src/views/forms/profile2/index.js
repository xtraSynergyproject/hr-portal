// ** MUI Components
import Grid from '@mui/material/Grid'

// ** Demo Components
import AboutOverivew from 'src/views/pages/user-profile/overview/AboutOverview'

const ProfileTab = ({ data }) => {
  return data && Object.values(data).length ? (
    <Grid >
      <Grid item xl={4} md={5} xs={12}>
        <AboutOverivew about={data.about} contacts={data.contacts} teams={data.teams} overview={data.overview} />
      </Grid>
    </Grid>
  ) : null
}

export default ProfileTab
