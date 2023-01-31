// ** React Imports
import { useState, useEffect } from 'react'

// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'

// ** Third Party Imports
import axios from 'axios'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import MainModal from './PayrollFormAndEditModals'

// *********************NAME ROLE AND DOJ BY SUFIYAN ANSARI BY API
function createData(PersonFullName, LocationName, PositionName, DateOfJoin) {
  return {
    PersonFullName,
    LocationName,
    PositionName,
    DateOfJoin
  }
}

const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

const PayrollUserProfileHeader = () => {
  // ***************API by ME

  const [myData, setMyData] = useState([])
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264'
      )
      .then(response => {
        setMyData(response.data)
        // console.log(response.myData, 'profile data')
      })
  }, [])

  // ** State
  const [data, setData] = useState(null)
  useEffect(() => {
    axios.get('/pages/profile-header').then(response => {
      setData(response.data)
    })
  }, [])
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'

  return data !== null ? (
    <>
      <Card>
        <CardMedia
          component='img'
          alt='profile-header'
          image='https://www.tnex.co.in/wp-content/uploads/2019/05/full-stack-banner-2.jpg'
          sx={{
            height: { xs: 150, md: 250 }
          }}
        />
        <CardContent
          sx={{
            pt: 0,
            mt: -8,
            display: 'flex',
            alignItems: 'flex-end',
            flexWrap: { xs: 'wrap', md: 'nowrap' },
            justifyContent: { xs: 'center', md: 'flex-start' }
          }}
        >
          <ProfilePicture
            src='https://eucg-consulting.com/images/uploads/verkhilio-falko-square-600x600.jpg'
            alt='profile-picture'
          />
          <Box
            sx={{
              width: '100%',
              display: 'flex',
              ml: { xs: 0, md: 6 },
              alignItems: 'flex-end',
              flexWrap: ['wrap', 'nowrap'],
              justifyContent: ['center', 'space-between']
            }}
          >
            <Box sx={{ mb: [6, 0], display: 'flex', flexDirection: 'column', alignItems: ['center', 'flex-start'] }}>
              <Typography variant='h5' sx={{ mb: 4 }}>
                {myData.PersonFullName}
              </Typography>
              <Box
                sx={{
                  display: 'flex',
                  flexWrap: 'wrap',
                  justifyContent: ['center', 'flex-start']
                }}
              >
                <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  <Icon icon={designationIcon} />
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                    {myData.PositionName}
                  </Typography>
                </Box>
                <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  <Icon icon='mdi:map-marker-outline' />
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                    {myData.LocationName}
                  </Typography>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  <Icon icon='mdi:calendar-blank' />
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{myData.DateOfJoin}</Typography>
                </Box>
              </Box>
            </Box>

            <Box>
              <MainModal/>
            </Box>
          </Box>
        </CardContent>
      </Card>
    </>
  ) : null
}

export default PayrollUserProfileHeader
