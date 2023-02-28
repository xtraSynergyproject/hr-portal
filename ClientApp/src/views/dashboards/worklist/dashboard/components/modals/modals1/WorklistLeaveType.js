// ** MUI Imports
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';




// Styled Grid component
const StyledGrid = styled(Grid)(({ theme }) => ({
    [theme.breakpoints.down('sm')]: {
        display: 'flex',
        justifyContent: 'center'
    }
}))

// Styled component for the image
const Img = styled('img')(({ theme }) => ({
    right: 13,
    bottom: 0,
    height: 200,
    position: 'absolute',
    [theme.breakpoints.down('sm')]: {
        height: 165,
        position: 'static'
    }
}))


const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
}));





const CardWelcomeBack = () => {
    return (
        

        <Box sx={{ height: 820, width:'60rem', background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>
          <Typography gutterBottom variant="h5" component="div" margin={8}>  Create New Leave Request </Typography>
          
                                            

            <Card sx={{ position: 'relative', overflow: 'visible', mt: { xs: 0, sm: 14.4, md: 0 } }}>
                <CardContent sx={{ p: theme => theme.spacing(7.25, 7.5, 7.75, 7.5) }}>


                    <Grid container rowSpacing={3}  columnSpacing={20}>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.LciTNhZjw8eFQOLLtG4mawHaGG?pid=ImgDet&rs=1"
                                    title="green iguana"
                                    
         


                                />
                                <Button size="small" >Compassionately Leave</Button>




                            </Item>
                        </Grid>

                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.BvzRC46VL0qmmwlbIA0WtAHaE7?w=254&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"

                                />
                                <Button size="small" >Un Absent</Button>



                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.Srv_C1WBdLgMg39gyRfxDgHaD4?w=282&h=180&c=7&r=0&o=5&pid=1.7"
                                    title="green iguana"
                                />
                                <Button size="small" >Leave Adjustement</Button>




                            </Item>
                        </Grid>

                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://www.signaturestaff.com.au/wp-content/uploads/2016/07/leave-cancellation.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Leave Cancel</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.7eTnY5DuuU1aNyJcCzyavQHaFj?pid=ImgDet&w=736&h=552&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Marriage Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.0Qtyk58q8SMzAu08QaBkTgHaFb?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Paternity Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://2.bp.blogspot.com/-6CMKoesdwZU/WgCoAzf3VdI/AAAAAAAAMqw/c3u_t2nJctUJCm7rYGG_TzLbBmv44cr7ACLcBGAs/s1600/Maternity2017.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Maternity Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://i.pinimg.com/736x/8b/1a/d8/8b1ad84e055b80262af92696b23fb116.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Annual Leave Encasement</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.0Y8fmMTjCc2L9YucYXQqKQHaFa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Sick Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.ZQC7DRRbK-_Ma56FwC9K1gHaE8?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Unpaid Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://blogs.glowscotland.org.uk/nl/public/CardinalNewmanWebsite/uploads/sites/21883/2017/05/study-leave.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Examination  Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.4i3AQu4B7vDF9-WSPd1-mQHaFP?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" > Leave HandOver Service</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://th.bing.com/th/id/OIP.SiC4WD4-GF10pxH-gaqdvgHaHa?pid=ImgDet&rs=1"
                                    title="green iguana"
                                />
                                <Button size="small" >Annual Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://i.ytimg.com/vi/c8NulVkRsHI/maxresdefault.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Sample Leave</Button>




                            </Item>
                        </Grid>
                        <Grid item xs={3}>
                            <Item>

                                <CardMedia
                                    sx={{ height: 140, width: 200 }}
                                    image="https://thumbs.dreamstime.com/b/business-man-doing-hard-work-office-22765456.jpg"
                                    title="green iguana"
                                />
                                <Button size="small" >Under Time Leave</Button>




                            </Item>
                        </Grid>



                    </Grid>


















                </CardContent>
            </Card>
        </Box>
    )
}

export default CardWelcomeBack
